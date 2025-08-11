using CodeAnalytics.Engine.Collector.Collectors.Interfaces;
using CodeAnalytics.Engine.Collector.Collectors.Options;
using CodeAnalytics.Engine.Extensions.Database;
using CodeAnalytics.Engine.Storage.Contexts;
using CodeAnalytics.Engine.Storage.Models.Structure;
using Me.Memory.Extensions;
using Me.Memory.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeAnalytics.Engine.Collector.Collectors;

public sealed partial class SolutionCollector : ISolutionCollector
{
   private readonly ILogger<SolutionCollector> _logger;
   
   private readonly SolutionOptions _options;
   private readonly WorkPool _workPool;

   private int _currentProjectCount;
   private int _maxProjectCount;
   
   public SolutionCollector(SolutionOptions options)
   {
      _logger = options.ServiceProvider.GetRequiredService<ILogger<SolutionCollector>>();
      
      _options = options;
      _workPool = new WorkPool(new WorkPoolOptions()
      {
         MaxDegreeOfParallelism = _options.MaxDegreeOfParallelism,
      });

      _currentProjectCount = 0;
   }
   
   public async Task Collect(CancellationToken ct = default)
   {
      using var pack = await Bootstrapper.OpenSolution(_options.Path, ct);
      var projects = pack.Solution.Projects.ToList();

      await using var dbContext = new DbMainContext(_options.DatabaseFilePath);
      LogUpdateProjectCount(_currentProjectCount, projects.Count);
      
      var solutionReference = await GetSolutionReference(dbContext, pack.Solution, ct);
      
      _maxProjectCount = projects.Count;
      List<Task<bool>> tasks = [];
      
      foreach (var project in projects)
      {
         tasks.Add(_workPool.Enqueue(GetCollectProjectFunc(
            solutionReference, project, pack.WorkSpace), ct));   
      }
      
      await Task.WhenAll(tasks)
         .WithAggregateException();
   }
   
   private Func<CancellationToken, Task<bool>> GetCollectProjectFunc(
      SolutionReference solutionReference, 
      Project project, MSBuildWorkspace workspace)
   {
      return (cancel) => CollectProject(solutionReference, project, workspace, cancel);
   }
   
   private async Task<bool> CollectProject(
      SolutionReference solutionReference, 
      Project project, MSBuildWorkspace workspace, CancellationToken ct)
   {
      var options = ProjectOptions.Create(_options);
      options.Path = project.FilePath ?? throw new InvalidOperationException();
      
      await using var collector = new ProjectCollector(options);
      var result = await collector.Collect(
         solutionReference, workspace, project, ct);
      
      Interlocked.Increment(ref _currentProjectCount);
      LogUpdateProjectCount(_currentProjectCount, _maxProjectCount);

      if (result is not { IsSuccess: true })
      {
         LogProjectError(project.FilePath, result.Error.Detail);
         return false;
      }

      return true;
   }

   private async Task<SolutionReference> GetSolutionReference(
      DbMainContext context,
      Solution solution,
      CancellationToken ct = default)
   {
      var relativePath = Path.GetRelativePath(_options.BasePath, solution.FilePath ?? _options.Path);
      
      return await context.GetOrInsert(context.SolutionReferences, new SolutionReference()
         {
            Name = Path.GetFileName(relativePath),
            RelativePath = relativePath,
            ProjectReferences = []
         },
         x => x.RelativePath == relativePath,
         ct);
   }
   
   public async ValueTask DisposeAsync()
   {
      await _workPool.DisposeAsync();
   }
}
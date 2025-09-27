using CodeAnalytics.Engine.Collectors.Options;
using CodeAnalytics.Engine.Storage.Common;
using CodeAnalytics.Engine.Storage.Extensions;
using CodeAnalytics.Engine.Storage.Models.Structure;
using Me.Memory.Extensions;
using Me.Memory.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeAnalytics.Engine.Collectors.Common;

public sealed partial class SolutionCollector : IAsyncDisposable
{
   private readonly ILogger<SolutionCollector> _logger;
   private readonly IDbContextFactory<DbMainContext> _dbContextFactory;
   private readonly IServiceProvider _serviceProvider;

   private readonly IOptionsMonitor<CollectorOptions> _optionsMonitor;
   private CollectorOptions CollectorOptions => _optionsMonitor.CurrentValue;
   
   private readonly WorkPool _workPool;
   
   private int _currentProjectCount;
   private int _maxProjectCount;

   public SolutionCollector(
      ILogger<SolutionCollector> logger,
      IOptionsMonitor<CollectorOptions> optionsMonitor,
      IDbContextFactory<DbMainContext> dbContextFactory,
      IServiceProvider serviceProvider)
   {
      _logger = logger;
      _optionsMonitor = optionsMonitor;
      _dbContextFactory = dbContextFactory;
      _serviceProvider = serviceProvider;

      _workPool = new WorkPool(new WorkPoolOptions()
      {
         MaxDegreeOfParallelism = CollectorOptions.MaxDegreeOfParallelism
      });
      _currentProjectCount = 0;
   }

   public async Task Collect(CancellationToken ct = default)
   {
      using var pack = await WorkspaceBootstrapper.OpenSolution(CollectorOptions.Path, ct);
      var projects = pack.Solution.Projects.ToList();

      await using var dbContext = await _dbContextFactory.CreateDbContextAsync(ct);
      LogUpdateProjectCount(_currentProjectCount, projects.Count);

      var dbSolution = await GetOrCreateDbSolution(dbContext, pack.Solution, ct);
      _maxProjectCount = projects.Count;
      
      List<Task<bool>> tasks = [];
      foreach (var project in projects)
      {
         var collectFunc = CreateCollectFunc(dbSolution, project, pack.WorkSpace);
         tasks.Add(_workPool.Enqueue(collectFunc, ct));   
      }
      
      await Task.WhenAll(tasks)
         .WithAggregateException();
   }

   private Func<CancellationToken, Task<bool>> CreateCollectFunc(
      DbSolution dbSolution, Project project, MSBuildWorkspace workSpace)
   {
      return (ct) => CollectProject(dbSolution, project, workSpace, ct);
   }

   private async Task<bool> CollectProject(
      DbSolution dbSolution, Project project, MSBuildWorkspace workSpace,
      CancellationToken ct)
   {
      await using var dbContext = await _dbContextFactory.CreateDbContextAsync(ct);
      
      await using var collector = _serviceProvider.GetRequiredService<ProjectCollector>();
      var result = await collector.Collect(dbContext, dbSolution, workSpace, project, ct);
      
      Interlocked.Increment(ref _currentProjectCount);
      LogUpdateProjectCount(_currentProjectCount, _maxProjectCount);

      if (result is { IsSuccess: true }) 
         return true;
      
      LogProjectError(
         project.FilePath ?? throw new InvalidOperationException(), 
         result.Error.Detail);
      
      return false;
   }
   
   private async Task<DbSolution> GetOrCreateDbSolution(
      DbMainContext dbContext,
      Solution solution,
      CancellationToken ct = default)
   {
      var relativePath = Path.GetRelativePath(
         CollectorOptions.BasePath, solution.FilePath ?? CollectorOptions.Path);
      var created = new DbSolution()
      {
         Name = Path.GetFileName(relativePath),
         RelativeFilePath = relativePath,
         Projects = []
      };

      return await dbContext.UpdateOrCreate(dbContext.Solutions)
         .Match(x => x.RelativeFilePath == relativePath)
         .OnCreate(() => created)
         .Execute(ct);
   }
   
   public async ValueTask DisposeAsync()
   {
      await _workPool.DisposeAsync();
   }
}
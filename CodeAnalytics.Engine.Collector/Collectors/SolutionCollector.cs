using CodeAnalytics.Engine.Collector.Collectors.Interfaces;
using CodeAnalytics.Engine.Collector.Collectors.Models;
using CodeAnalytics.Engine.Collector.Collectors.Options;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Common.Extensions;
using CodeAnalytics.Engine.Common.Threading.Pools;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Merges.Common;
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

   public async Task<CollectorStore> Collect(CancellationToken ct = default)
   {
      var (workSpace, solution) = await Bootstrapper.OpenSolution(_options.Path, ct);
      using var workspace = workSpace;
      var projects = solution.Projects.ToList();
      
      LogUpdateProjectCount(_currentProjectCount, projects.Count);

      _maxProjectCount = projects.Count;
      List<Task<CollectorStore?>> tasks = [];
      
      foreach (var project in projects)
      {
         tasks.Add(_workPool.Enqueue(CollectProjectTask(project, workspace), ct));   
      }

      await Task.WhenAll(tasks)
         .WithAggregateException();

      var result = new CollectorStore()
      {
         NodeIdStore = _options.NodeIdStore,
         StringIdStore = _options.StringIdStore,
         Occurrences = _options.Occurrences,
         ComponentStore = new MergableComponentStore(_options.InitialCapacityPerComponentPool),
         LineCountStore = new LineCountStore(),
         Projects = []
      };

      foreach (var task in tasks)
      {
         if (await task is { } project)
         {
            result.Merge(project);
         }
      }
      
      _options.Occurrences.Clean(
         result.ComponentStore.GetOrCreatePool<SymbolComponent, SymbolMerger>(),
         result);
      
      return result;
   }

   private Func<CancellationToken, Task<CollectorStore?>> CollectProjectTask(
      Project project, MSBuildWorkspace workspace)
   {
      return (cancel) => CollectProject(project, workspace, cancel);
   }
   
   private async Task<CollectorStore?> CollectProject(
      Project project, MSBuildWorkspace workspace, CancellationToken ct)
   {
      var options = ProjectOptions.Create(_options);
      options.Path = project.FilePath ?? throw new InvalidOperationException();
      
      var collector = new ProjectCollector(options);
      var result = await collector.Collect(new ProjectParseInfo()
      {
         Compilation = await project.GetCompilationAsync(ct),
         Workspace = workspace
      }, ct);
      
      Interlocked.Increment(ref _currentProjectCount);
      LogUpdateProjectCount(_currentProjectCount, _maxProjectCount);

      if (result is not { IsSuccess: true, Success: { } success })
      {
         LogProjectError(project.FilePath, result.Error.Detail);
         return null;
      }

      return success;
   }
   
   public async ValueTask DisposeAsync()
   {
      await _workPool.DisposeAsync();
   }
}
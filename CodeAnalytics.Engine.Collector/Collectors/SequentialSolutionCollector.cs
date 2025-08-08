using CodeAnalytics.Engine.Collector.Collectors.Interfaces;
using CodeAnalytics.Engine.Collector.Collectors.Models;
using CodeAnalytics.Engine.Collector.Collectors.Options;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeAnalytics.Engine.Collector.Collectors;

/// <summary>
/// Keeps the solution open and therefore has no overhead opening each project on its own
/// </summary>
public sealed partial class SequentialSolutionCollector : ISolutionCollector
{
   private readonly ILogger<SequentialSolutionCollector> _logger;
   private readonly SolutionOptions _options;

   private int _currentProjectCount;
   private int _maxProjectCount;
   
   public SequentialSolutionCollector(SolutionOptions options)
   {
      _logger = options.ServiceProvider.GetRequiredService<ILogger<SequentialSolutionCollector>>();
      
      _options = options;
      _currentProjectCount = 0;
   }
   
   public async Task<CollectorStore> Collect(CancellationToken ct = default)
   {
      var (workSpace, solution) = await Bootstrapper.OpenSolution(_options.Path, ct);
      using var workspace = workSpace;
      
      var projects = solution.Projects.ToList();
      _maxProjectCount = projects.Count;
      LogUpdateProjectCount(_currentProjectCount, _maxProjectCount);
      
      var ret = new CollectorStore()
      {
         NodeIdStore = _options.NodeIdStore,
         StringIdStore = _options.StringIdStore,
         Occurrences = _options.Occurrences,
         ComponentStore = new MergableComponentStore(_options.InitialCapacityPerComponentPool),
         LineCountStore = new LineCountStore(),
         Projects = []
      };
      var options = ProjectOptions.Create(_options);

      foreach (var project in projects)
      {
         options.Path = project.FilePath ?? throw new InvalidOperationException();
         
         var collector = new ProjectCollector(options);
         var result = await collector.Collect(new ProjectParseInfo()
         {
            Compilation = await project.GetCompilationAsync(ct),
            Workspace = workspace
         }, ct);
         
         LogUpdateProjectCount(++_currentProjectCount, _maxProjectCount);
         if (result is not { IsSuccess: true, Success: { } success })
         {
            LogProjectError(options.Path, result.Error.Detail);
            continue;
         }

         ret.Merge(success);
      }
      
      return ret;
   }
   
   public ValueTask DisposeAsync()
   {
      return ValueTask.CompletedTask;
   }
}
using Beskar.CodeAnalytics.Collector.Options;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Me.Memory.Extensions;
using Me.Memory.Threading;
using Me.Memory.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Beskar.CodeAnalytics.Collector.Projects;

public sealed partial class SolutionCollector : IAsyncDisposable
{
   private readonly CsSolutionHandle _handle;
   private readonly WorkPool _workPool;
   private readonly CollectorOptions _options;
   
   private readonly ILogger<SolutionCollector> _logger;
   private readonly ILoggerFactory _loggerFactory;
   
   private int _projectCount;
   private int _currentProjectCount;
   
   private string? _solutionName;
   
   public SolutionCollector(
      CsSolutionHandle solutionHandle,
      CollectorOptions collectorOptions,
      ILogger<SolutionCollector> logger,
      ILoggerFactory loggerFactory)
   {
      _handle = solutionHandle;
      _options = collectorOptions;
      
      _loggerFactory = loggerFactory;
      _logger = logger;

      _workPool = new WorkPool(new WorkPoolOptions()
      {
         MaxDegreeOfParallelism = _options.MaxDegreeOfParallelism,
      });
   }
   
   public async Task Discover(DiscoveryBatch batch, CancellationToken ct = default)
   {
      _solutionName = Path.GetFileName(_handle.Solution.FilePath) ?? _handle.Solution.Id.ToString() ?? "Unknown";
      LogStart(_solutionName);

      var totalTimerResult = new AsyncTimerResult();
      var totalTimer = new AsyncTimer(totalTimerResult);
      
      var projects = _handle.Solution.Projects
         .Where(x => x.SupportsCompilation).ToArray();
      _projectCount = projects.Length;

      var tasks = new Task<bool>[_projectCount];
      for (var index = 0; index < projects.Length; index++)
      {
         var project = projects[index];
         tasks[index] = _workPool.Enqueue(DiscoverProjectTask(project, batch), ct);
      }
      
      await Task.WhenAll(tasks)
         .WithAggregateException();
      
      totalTimer.Dispose();
      LogStop(_solutionName, totalTimerResult.Elapsed);
   }

   private Func<CancellationToken, Task<bool>> DiscoverProjectTask(
      Project project, DiscoveryBatch batch)
   {
      return (ct) => DiscoverProject(project, batch, ct);
   }

   private async Task<bool> DiscoverProject(
      Project project, DiscoveryBatch batch, CancellationToken ct)
   {
      var collector = new ProjectCollector(
         new CsProjectHandle(_handle, project),
         _loggerFactory.CreateLogger<ProjectCollector>());

      await collector.Discover(batch, ct);
      
      Interlocked.Increment(ref _currentProjectCount);
      LogProgress(_solutionName ?? string.Empty, 
         _currentProjectCount, _projectCount);
      
      return true;
   }
   
   public async ValueTask DisposeAsync()
   {
      await _workPool.DisposeAsync();
   }
}
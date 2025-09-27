using CodeAnalytics.Engine.Collectors.Options;
using CodeAnalytics.Engine.Storage.Models.Structure;
using Me.Memory.Results;
using Me.Memory.Results.Errors;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeAnalytics.Engine.Collectors.Common;

public sealed partial class ProjectCollector
{
   private readonly ILogger<ProjectCollector> _logger;

   private readonly IOptionsMonitor<CollectorOptions> _optionsMonitor;
   private CollectorOptions CollectorOptions => _optionsMonitor.CurrentValue;

   public ProjectCollector(
      ILogger<ProjectCollector> logger, 
      IOptionsMonitor<CollectorOptions> optionsMonitor)
   {
      _logger = logger;
      _optionsMonitor = optionsMonitor;
   }

   public async Task<Result<int, Error<string>>> Collect(
      DbSolution dbSolution,
      Workspace workSpace,
      Project project,
      CancellationToken ct)
   {
      return new Error<string>("a");
   }
}
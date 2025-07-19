using Microsoft.Extensions.Logging;

namespace CodeAnalytics.Engine.Collector.Collectors;

public sealed partial class ProjectCollector
{
   [LoggerMessage(
      EventId = 0,
      Level = LogLevel.Information,
      Message = "Start collecting project {ProjectPath}."
   )]
   private partial void LogStartProjectCollect(string projectPath);
   
   [LoggerMessage(
      EventId = 1,
      Level = LogLevel.Information,
      Message = "Ran through {Count} nodes in project."
   )]
   private partial void LogNodesRan(long count);
}
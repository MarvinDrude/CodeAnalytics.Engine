using Microsoft.Extensions.Logging;

namespace CodeAnalytics.Engine.Collectors.Common;

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
      Message = "Ran through {Count} nodes in project. Took: {LoadingTime}."
   )]
   private partial void LogNodesRan(long count, TimeSpan loadingTime);
   
   [LoggerMessage(
      EventId = 2,
      Level = LogLevel.Information,
      Message = "Project startup time: {LoadingTime}."
   )]
   private partial void LogStartupTime(TimeSpan loadingTime);
}
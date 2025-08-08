using Microsoft.Extensions.Logging;

namespace CodeAnalytics.Engine.Collector.Collectors;

public sealed partial class SequentialSolutionCollector
{
   [LoggerMessage(
      EventId = 0,
      Level = LogLevel.Information,
      Message = "Currently at {CurrentCount}/{MaxCount} projects."
   )]
   private partial void LogUpdateProjectCount(int currentCount, int maxCount);
   
   [LoggerMessage(
      EventId = 1,
      Level = LogLevel.Error,
      Message = "Error at collecting from project: {Path}. {Error}"
   )]
   private partial void LogProjectError(string path, string error);
}
using Microsoft.Extensions.Logging;

namespace Beskar.CodeAnalytics.Collector.Projects;

public sealed partial class SolutionCollector
{
   [LoggerMessage(LogLevel.Information, "[S:{SolutionName}] Starting the discovery process...")]
   private partial void LogStart(string solutionName);
   
   [LoggerMessage(LogLevel.Information, "[S:{SolutionName}] Discovery progress {Current} / {Max}.")]
   private partial void LogProgress(string solutionName, int current, int max);
   
   [LoggerMessage(LogLevel.Information, "[S:{SolutionName}] Discovery finished and took: {Time}.")]
   private partial void LogStop(string solutionName, TimeSpan time);
}
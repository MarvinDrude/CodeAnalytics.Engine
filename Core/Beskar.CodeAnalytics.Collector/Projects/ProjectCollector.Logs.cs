using Microsoft.Extensions.Logging;

namespace Beskar.CodeAnalytics.Collector.Projects;

public sealed partial class ProjectCollector
{
   [LoggerMessage(LogLevel.Information, "[P:{ProjectName}] Starting the discovery process...")]
   private partial void LogStart(string projectName);
   
   [LoggerMessage(LogLevel.Information, "[P:{ProjectName}] Getting the compilation took: {Time}.")]
   private partial void LogCompilationTime(string projectName, TimeSpan time);
   
   [LoggerMessage(LogLevel.Information, "[P:{ProjectName}] Discovery finished and took: {Time}.")]
   private partial void LogStop(string projectName, TimeSpan time);
}
using Microsoft.Extensions.Logging;

namespace Beskar.CodeAnalytics.Data.Bake.Common;

public sealed partial class BakeEngine
{
   [LoggerMessage(LogLevel.Information, "[Bake][{StepName}] Running the step...")]
   private partial void LogStartStep(string stepName);
   
   [LoggerMessage(LogLevel.Information, "[Bake][{StepName}] Finished with duration of {Duration}.")]
   private partial void LogStopStep(string stepName, TimeSpan duration);
}
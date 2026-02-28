using Microsoft.Extensions.Logging;

namespace Beskar.CodeAnalytics.Data.Bake.Steps;

public sealed partial class EdgeConnectionBakeStep
{
   [LoggerMessage(LogLevel.Information, "{Name} took {Duration} to complete.")]
   private partial void LogStep(string name, TimeSpan duration);
}
namespace Beskar.CodeAnalytics.Data.Analytics.Internal.Models;

public sealed class PipelineContext
{
   internal int TotalCallCount { get; set; }
   
   internal int CacheHitCount { get; set; }
}
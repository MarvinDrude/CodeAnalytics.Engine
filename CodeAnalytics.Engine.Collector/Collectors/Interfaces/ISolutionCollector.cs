using CodeAnalytics.Engine.Collectors;

namespace CodeAnalytics.Engine.Collector.Collectors.Interfaces;

public interface ISolutionCollector : IAsyncDisposable
{
   public Task<CollectorStore> Collect(CancellationToken ct = default);
}
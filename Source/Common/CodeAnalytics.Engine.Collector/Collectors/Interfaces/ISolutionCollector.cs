namespace CodeAnalytics.Engine.Collector.Collectors.Interfaces;

public interface ISolutionCollector : IAsyncDisposable
{
   public Task Collect(CancellationToken ct = default);
}
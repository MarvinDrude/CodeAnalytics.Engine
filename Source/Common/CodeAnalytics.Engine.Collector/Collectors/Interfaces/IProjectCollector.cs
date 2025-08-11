using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Collectors.Interfaces;

public interface IProjectCollector : IAsyncDisposable
{
   public Task Collect(Project project, CancellationToken ct = default);
}
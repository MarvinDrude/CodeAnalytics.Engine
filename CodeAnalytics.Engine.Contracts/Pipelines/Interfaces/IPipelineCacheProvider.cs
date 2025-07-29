using System.Diagnostics.CodeAnalysis;

namespace CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;

public interface IPipelineCacheProvider
{
   public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T value);
   public void Set<T>(string key, T value);
   public void Invalidate(string key);
}
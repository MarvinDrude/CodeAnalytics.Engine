using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;

namespace CodeAnalytics.Engine.Pipelines.Cache;

public sealed class MemoryPipelineCacheProvider : IPipelineCacheProvider
{
   private readonly ConcurrentDictionary<string, ICacheEntry> _cache = [];
   
   public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T value)
   {
      if (_cache.TryGetValue(key, out var entry)
          && entry is CacheEntry<T> concrete)
      {
         value = concrete.Value;
         return true;
      }

      value = default;
      return false;
   }

   public void Set<T>(string key, T value)
   {
      _cache[key] = new CacheEntry<T>()
      {
         Value = value
      };
   }

   public void Invalidate(string key)
   {
      _cache.TryRemove(key, out _);
   }

   private sealed class CacheEntry<T> : ICacheEntry
   {
      public required T Value { get; init; }
   }

   private interface ICacheEntry;
}
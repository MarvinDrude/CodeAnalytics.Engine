using System.Collections.Concurrent;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Collectors.Caches;

public sealed class SymbolIdCache
{
   private readonly ConcurrentDictionary<string, DbSymbolId> _cache = [];

   public bool TryGetId(string hashId, out DbSymbolId id)
   {
      return this._cache.TryGetValue(hashId, out id);
   }

   public void Set(string hashId, DbSymbolId id)
   {
      _cache[hashId] = id;
   }
}
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Ids.Interfaces;

namespace CodeAnalytics.Engine.Ids;

public class IdStoreBase : IIdStore
{
   public int NextId => _nextId;
   public string Name { get; }
   
   private readonly ConcurrentDictionary<string, Lazy<int>> _map = [];
   private readonly ConcurrentDictionary<int, string> _reverse = [];
   
   private int _nextId;

   public IdStoreBase(string name)
   {
      Name = name;
      _nextId = 0;
   }

   public IdStoreBase(
      string name,
      Dictionary<string, int> map,
      int nextId)
   {
      foreach (var (key, value) in map)
      {
         RestoreEntry(key, value);
      }

      Name = name;
      _nextId = nextId;
   }
   
   public string? GetById(int id)
   {
      return _reverse.GetValueOrDefault(id);
   }

   public bool TryGetById(int id, [MaybeNullWhen(false)] out string value)
   {
      return _reverse.TryGetValue(id, out value);
   }

   public int GetOrAddId(string value)
   {
      var lazy = _map.GetOrAdd(value, _ => new Lazy<int>(
         () => Interlocked.Increment(ref _nextId),
         LazyThreadSafetyMode.ExecutionAndPublication));
      var id = lazy.Value;

      _reverse.TryAdd(id, value);
      return id;
   }

   public Dictionary<string, int> ToDictionary()
   {
      return _map.ToDictionary(x => x.Key, x => x.Value.Value);
   }
   
   private void RestoreEntry(string text, int id)
   {
      _map[text] = new Lazy<int>(() => id, LazyThreadSafetyMode.ExecutionAndPublication);
      _reverse[id] = text;
   }
}
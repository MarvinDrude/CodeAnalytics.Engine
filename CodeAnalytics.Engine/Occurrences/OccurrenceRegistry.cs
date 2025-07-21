using System.Collections.Concurrent;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Occurrences;

namespace CodeAnalytics.Engine.Occurrences;

public sealed class OccurrenceRegistry
{
   private readonly ConcurrentDictionary<NodeId, Lazy<GlobalOccurrence>> _byNodes = [];

   public OccurrenceRegistry()
   {
      
   }

   public OccurrenceRegistry(Dictionary<NodeId, GlobalOccurrence> dict)
   {
      foreach (var (key, value) in dict)
      {
         CreateKeyValue(key, value);
      }
   }

   public GlobalOccurrence GetOrCreate(NodeId nodeId)
   {
      var lazy = _byNodes.GetOrAdd(nodeId, _ => new Lazy<GlobalOccurrence>(
         () => new GlobalOccurrence(),
         LazyThreadSafetyMode.ExecutionAndPublication));
      
      return lazy.Value;
   }

   public Dictionary<NodeId, GlobalOccurrence> ToDictionary()
   {
      return _byNodes.ToDictionary(x => x.Key, x => x.Value.Value);
   }

   private void CreateKeyValue(NodeId nodeId, GlobalOccurrence occurrence)
   {
      _byNodes[nodeId] = new Lazy<GlobalOccurrence>(() => occurrence);
   }
}
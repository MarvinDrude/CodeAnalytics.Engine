using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Ids;
using CodeAnalytics.Engine.Serialization.Ids;
using CodeAnalytics.Engine.Serialization.Occurrence;

namespace CodeAnalytics.Engine.Serialization.Stores;

public sealed class CollectorStoreSerializer : ISerializer<CollectorStore>
{
   public static void Serialize(ref ByteWriter writer, ref CollectorStore ob)
   {
      var idStringStore = ob.StringIdStore;
      var idNodeStore = ob.NodeIdStore;

      var lineCountStore = ob.LineCountStore;
      var componentStore = ob.ComponentStore;

      StringIdStoreSerializer.Serialize(ref writer, ref idStringStore);
      NodeIdStoreSerializer.Serialize(ref writer, ref idNodeStore);
      
      LineCountStoreSerializer.Serialize(ref writer, ref lineCountStore);
      MergableComponentStoreSerializer.Serialize(ref writer, ref componentStore);
      
      var occurrences = ob.Occurrences;
      OccurrenceRegistrySerializer.Serialize(ref writer, ref occurrences);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out CollectorStore ob)
   {
      if (!StringIdStoreSerializer.TryDeserialize(ref reader, out var idStringStore)
          || !NodeIdStoreSerializer.TryDeserialize(ref reader, out var idNodeStore))
      {
         ob = null;
         return false;
      }
      
      StringIdStore.Current = idStringStore;
      NodeIdStore.Current = idNodeStore;

      if (!LineCountStoreSerializer.TryDeserialize(ref reader, out var lineCountStore)
          || !MergableComponentStoreSerializer.TryDeserialize(ref reader, out var componentStore))
      {
         ob = null;
         return false;
      }

      if (!OccurrenceRegistrySerializer.TryDeserialize(ref reader, out var occurrences))
      {
         ob = null;
         return false;
      }

      ob = new CollectorStore()
      {
         StringIdStore = idStringStore,
         NodeIdStore = idNodeStore,
         ComponentStore = componentStore,
         LineCountStore = lineCountStore,
         Occurrences = occurrences
      };
      
      StringIdStore.Current = null;
      NodeIdStore.Current = null;

      return true;
   }
}
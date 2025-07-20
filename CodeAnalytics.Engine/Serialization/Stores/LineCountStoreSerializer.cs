using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Collectors;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;
using CodeAnalytics.Engine.Serialization.System.Collections;

namespace CodeAnalytics.Engine.Serialization.Stores;

public sealed class LineCountStoreSerializer : ISerializer<LineCountStore>
{
   public static void Serialize(ref ByteWriter writer, ref LineCountStore ob)
   {
      var perNode = ob.LineCountsPerNode;
      var perFile = ob.LineCountsPerFile;
      
      DictionarySerializer<StringId, StringIdSerializer, LineCountStats, LineCountStatsSerializer>
         .Serialize(ref writer, ref perFile);
      
      DictionarySerializer<NodeId, NodeIdSerializer, LineCountNode, LineCountNodeSerializer>
         .Serialize(ref writer, ref perNode);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out LineCountStore ob)
   {
      if (!DictionarySerializer<StringId, StringIdSerializer, LineCountStats, LineCountStatsSerializer>
             .TryDeserialize(ref reader, out var perFile))
      {
         ob = null;
         return false;
      }
      
      if (!DictionarySerializer<NodeId, NodeIdSerializer, LineCountNode, LineCountNodeSerializer>
             .TryDeserialize(ref reader, out var perNode))
      {
         ob = null;
         return false;
      }

      ob = new LineCountStore(perNode, perFile);
      return true;
   }
}
using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Occurrences;
using CodeAnalytics.Engine.Serialization.Ids;
using CodeAnalytics.Engine.Serialization.System.Collections;

namespace CodeAnalytics.Engine.Serialization.Occurrence;

public sealed class OccurrenceRegistrySerializer : ISerializer<OccurrenceRegistry>
{
   public static void Serialize(ref ByteWriter writer, ref OccurrenceRegistry ob)
   {
      var dict = ob.ToDictionary();
      
      DictionarySerializer<NodeId, NodeIdSerializer, GlobalOccurrence, GlobalOccurrenceSerializer>
         .Serialize(ref writer, ref dict);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out OccurrenceRegistry ob)
   {
      ob = null;

      if (!DictionarySerializer<NodeId, NodeIdSerializer, GlobalOccurrence, GlobalOccurrenceSerializer>
            .TryDeserialize(ref reader, out var dict))
      {
         return false;
      }
      
      ob = new OccurrenceRegistry(dict);
      return true;
   }
}
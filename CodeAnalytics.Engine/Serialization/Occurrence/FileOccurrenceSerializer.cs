using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;
using CodeAnalytics.Engine.Serialization.System.Collections;

namespace CodeAnalytics.Engine.Serialization.Occurrence;

public sealed class FileOccurrenceSerializer : ISerializer<FileOccurrence>
{
   public static void Serialize(ref ByteWriter writer, ref FileOccurrence ob)
   {
      var pathId = ob.PathId;
      StringIdSerializer.Serialize(ref writer, ref pathId);

      var list = ob.LineOccurrences;
      ListSerializer<NodeOccurrence, NodeOccurrenceSerializer>.Serialize(ref writer, ref list);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out FileOccurrence ob)
   {
      ob = null;
      
      if (!StringIdSerializer.TryDeserialize(ref reader, out var pathId))
      {
         return false;
      }

      if (!ListSerializer<NodeOccurrence, NodeOccurrenceSerializer>.TryDeserialize(ref reader, out var list))
      {
         return false;
      }

      ob = new FileOccurrence()
      {
         PathId = pathId,
         LineOccurrences = list
      };
      return true;
   }
}
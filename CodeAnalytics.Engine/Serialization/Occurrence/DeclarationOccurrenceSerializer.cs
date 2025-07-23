using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Occurrence;

public sealed class DeclarationOccurrenceSerializer : ISerializer<DeclarationOccurrence>
{
   public static void Serialize(ref ByteWriter writer, ref DeclarationOccurrence ob)
   {
      var nodeId = ob.NodeId;
      NodeIdSerializer.Serialize(ref writer, ref nodeId);
      
      var fileId = ob.FileId;
      StringIdSerializer.Serialize(ref writer, ref fileId);
      
      writer.WriteLittleEndian(ob.LineNumber);
      writer.WriteLittleEndian(ob.SpanIndex);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out DeclarationOccurrence ob)
   {
      if (!NodeIdSerializer.TryDeserialize(ref reader, out var nodeId) 
          || !StringIdSerializer.TryDeserialize(ref reader, out var fileId))
      {
         ob = null;
         return false;
      }

      ob = new DeclarationOccurrence()
      {
         NodeId = nodeId,
         FileId = fileId,
         LineNumber = reader.ReadLittleEndian<int>(),
         SpanIndex = reader.ReadLittleEndian<int>()
      };

      return true;
   }
}
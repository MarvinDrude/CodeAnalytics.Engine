using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Contracts.TextRendering;
using CodeAnalytics.Engine.Serialization.System.Collections;
using CodeAnalytics.Engine.Serialization.TextRendering;

namespace CodeAnalytics.Engine.Serialization.Occurrence;

public sealed class NodeOccurrenceSerializer : ISerializer<NodeOccurrence>
{
   public static void Serialize(ref ByteWriter writer, ref NodeOccurrence ob)
   {
      writer.WriteLittleEndian(ob.SpanIndex);
      writer.WriteByte(ob.Flags);

      var list = ob.LineSpans;
      ListSerializer<SyntaxSpan, SyntaxSpanSerializer>.Serialize(ref writer, ref list);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out NodeOccurrence ob)
   {
      var spanIndex = reader.ReadLittleEndian<int>();
      var flags = reader.ReadByte();

      if (!ListSerializer<SyntaxSpan, SyntaxSpanSerializer>.TryDeserialize(ref reader, out var spans))
      {
         ob = null;
         return false;
      }

      ob = new NodeOccurrence()
      {
         Flags = flags,
         LineSpans = spans,
         SpanIndex = spanIndex
      };
      
      return true;
   }
}
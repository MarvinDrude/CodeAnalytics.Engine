using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Contracts.TextRendering;
using CodeAnalytics.Engine.Serialization.Ids;
using CodeAnalytics.Engine.Serialization.System.Common;

namespace CodeAnalytics.Engine.Serialization.TextRendering;

public sealed class SyntaxSpanSerializer : ISerializer<SyntaxSpan>
{
   public static void Serialize(ref ByteWriter writer, ref SyntaxSpan ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Reference);
      StringSerializer.Serialize(ref writer, ref ob.StringReference);

      var rawText = ob.RawText;
      StringSerializer.Serialize(ref writer, ref rawText);
      var color = ob.Color;
      StringSerializer.Serialize(ref writer, ref color);

      writer.WriteByte(ob.Flags.RawByte);
   }

   public static bool TryDeserialize(ref ByteReader reader, out SyntaxSpan ob)
   {
      ob = new SyntaxSpan();
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Reference)
          || !StringSerializer.TryDeserialize(ref reader, out var strReference))
      {
         return false;
      }

      ob.StringReference = strReference;

      if (!StringSerializer.TryDeserialize(ref reader, out var rawText)
          || !StringSerializer.TryDeserialize(ref reader, out var color))
      {
         return false;
      }
      
      ob.RawText = rawText;
      ob.Color = color;

      ob.Flags = new PackedBools(reader.ReadByte());
      
      return true;
   }
}
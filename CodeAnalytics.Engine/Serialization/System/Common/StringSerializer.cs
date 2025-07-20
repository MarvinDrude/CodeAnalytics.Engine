using System.Diagnostics.CodeAnalysis;
using System.Text;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.System.Common;

public sealed class StringSerializer : ISerializer<string>
{
   private static readonly Encoding _encoding = Encoding.UTF8;
   
   public static void Serialize(ref ByteWriter writer, ref string ob)
   {
      var span = ob.AsSpan();
      var length = _encoding.GetByteCount(span);

      writer.WriteLittleEndian(length);
      if (length > 0)
      {
         writer.WriteString(span, _encoding);
      }
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out string ob)
   {
      var length = reader.ReadLittleEndian<int>();

      ob = length > 0
         ? reader.ReadString(length, _encoding)
         : string.Empty;
      
      return true;
   }
}
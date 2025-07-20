using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Collectors;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.Stores;

public sealed class LineCountStatsSerializer : ISerializer<LineCountStats>
{
   public static void Serialize(ref ByteWriter writer, ref LineCountStats ob)
   {
      writer.WriteLittleEndian(ob.CodeCount);
      writer.WriteLittleEndian(ob.LineCount);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out LineCountStats ob)
   {
      ob = new LineCountStats
      {
         CodeCount = reader.ReadLittleEndian<int>(),
         LineCount = reader.ReadLittleEndian<int>()
      };

      return true;
   }
}
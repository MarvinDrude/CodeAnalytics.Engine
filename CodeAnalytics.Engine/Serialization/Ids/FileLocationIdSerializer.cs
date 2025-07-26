using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.Ids;

public sealed class FileLocationIdSerializer : ISerializer<FileLocationId>
{
   public static void Serialize(ref ByteWriter writer, ref FileLocationId ob)
   {
      var fileId = ob.FileId;
      StringIdSerializer.Serialize(ref writer, ref fileId);

      writer.WriteLittleEndian(ob.SpanIndex);
   }

   public static bool TryDeserialize(ref ByteReader reader, out FileLocationId ob)
   {
      if (!StringIdSerializer.TryDeserialize(ref reader, out var fileId))
      {
         ob = default;
         return false;
      }

      ob = new FileLocationId(
         fileId,
         reader.ReadLittleEndian<int>());
      
      return true;
   }
}
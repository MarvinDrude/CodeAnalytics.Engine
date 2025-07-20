using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Ids;

namespace CodeAnalytics.Engine.Serialization.Ids;

public sealed class StringIdSerializer : ISerializer<StringId>
{
   public static void Serialize(ref ByteWriter writer, ref StringId ob)
   {
      writer.WriteLittleEndian(ob.Value);
   }

   public static bool TryDeserialize(ref ByteReader reader, out StringId ob)
   {
      ob = new StringId(reader.ReadLittleEndian<int>(), StringIdStore.Current);
      return true;
   }
}
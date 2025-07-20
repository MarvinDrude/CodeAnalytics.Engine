using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Ids;

namespace CodeAnalytics.Engine.Serialization.Ids;

public sealed class NodeIdSerializer : ISerializer<NodeId>
{
   public static void Serialize(ref ByteWriter writer, ref NodeId ob)
   {
      writer.WriteLittleEndian(ob.Value);
   }

   public static bool TryDeserialize(ref ByteReader reader, out NodeId ob)
   {
      ob = new NodeId(reader.ReadLittleEndian<int>(), NodeIdStore.Current);
      return true;
   }
}
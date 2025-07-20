using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Types;

public sealed class StructSerializer : ISerializer<StructComponent>
{
   public static void Serialize(ref ByteWriter writer, ref StructComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
      writer.WriteByte(ob.Flags.RawByte);
   }

   public static bool TryDeserialize(ref ByteReader reader, out StructComponent ob)
   {
      ob = new StructComponent();
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id))
      {
         return false;
      }

      ob.Flags = new PackedBools(reader.ReadByte());
      
      return true;
   }
}
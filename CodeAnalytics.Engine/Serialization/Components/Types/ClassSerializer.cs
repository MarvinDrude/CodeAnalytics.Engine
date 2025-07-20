using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Types;

public sealed class ClassSerializer : ISerializer<ClassComponent>
{
   public static void Serialize(ref ByteWriter writer, ref ClassComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
      NodeIdSerializer.Serialize(ref writer, ref ob.BaseClassId);
      
      writer.WriteByte(ob.Flags.RawByte);
   }

   public static bool TryDeserialize(ref ByteReader reader, out ClassComponent ob)
   {
      ob = new ClassComponent();
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id))
      {
         return false;
      }

      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.BaseClassId))
      {
         return false;
      }

      ob.Flags = new PackedBools(reader.ReadByte());
      return true;
   }
}
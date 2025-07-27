using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Types;

public sealed class EnumValueSerializer : ISerializer<EnumValueComponent>
{
   public static void Serialize(ref ByteWriter writer, ref EnumValueComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
      StringIdSerializer.Serialize(ref writer, ref ob.Name);
      
      writer.WriteByte(ob.Flags.RawByte);

      if (ob.IsULong)
      {
         writer.WriteLittleEndian(ob.UValue);
      }
      else
      {
         writer.WriteLittleEndian(ob.Value);
      }
   }

   public static bool TryDeserialize(ref ByteReader reader, out EnumValueComponent ob)
   {
      if (!NodeIdSerializer.TryDeserialize(ref reader, out var id)
          || !StringIdSerializer.TryDeserialize(ref reader, out var name))
      {
         ob = default;
         return false;
      }

      var flags = new PackedBools(reader.ReadByte());
      ob = new EnumValueComponent()
      {
         Id = id,
         Name = name,
         Flags = flags
      };
      
      if (ob.IsULong)
      {
         ob.UValue = reader.ReadLittleEndian<ulong>();
      }
      else
      {
         ob.Value = reader.ReadLittleEndian<long>();
      }
      
      return true;
   }
}
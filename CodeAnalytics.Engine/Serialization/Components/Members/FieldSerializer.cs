using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Members;

public sealed class FieldSerializer : ISerializer<FieldComponent>
{
   public static void Serialize(ref ByteWriter writer, ref FieldComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
      
      writer.WriteByte(ob.Flags.RawByte);
   }

   public static bool TryDeserialize(ref ByteReader reader, out FieldComponent ob)
   {
      ob = new FieldComponent();
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id))
      {
         return false;
      }

      ob.Flags = new PackedBools(reader.ReadByte());
      
      return true;
   }
}
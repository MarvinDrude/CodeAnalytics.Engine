using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Members;

public sealed class PropertySerializer : ISerializer<PropertyComponent>
{
   public static void Serialize(ref ByteWriter writer, ref PropertyComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
      
      writer.WriteByte(ob.Flags.RawByte);
      
      writer.WriteLittleEndian(ob.GetterCyclomaticComplexity);
      writer.WriteLittleEndian(ob.SetterCyclomaticComplexity);
   }

   public static bool TryDeserialize(ref ByteReader reader, out PropertyComponent ob)
   {
      ob = new PropertyComponent();
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id))
      {
         return false;
      }

      ob.Flags = new PackedBools(reader.ReadByte());

      ob.GetterCyclomaticComplexity = reader.ReadLittleEndian<int>();
      ob.SetterCyclomaticComplexity = reader.ReadLittleEndian<int>();
      
      return true;
   }
}
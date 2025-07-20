using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Enums.Components;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Collections;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Members;

public sealed class ParameterSerializer : ISerializer<ParameterComponent>
{
   public static void Serialize(ref ByteWriter writer, ref ParameterComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
      NodeIdSerializer.Serialize(ref writer, ref ob.TypeId);

      writer.WriteLittleEndian(ob.Modifiers);
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.AttributeIds);
   }

   public static bool TryDeserialize(ref ByteReader reader, out ParameterComponent ob)
   {
      ob = new ParameterComponent();
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id)
          || !NodeIdSerializer.TryDeserialize(ref reader, out ob.TypeId))
      {
         return false;
      }

      ob.Modifiers = reader.ReadLittleEndian<ParameterModifier>();
      if (!PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.AttributeIds))
      {
         return false;
      }
      
      return true;
   }
}
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Collections;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Types;

public sealed class EnumSerializer : ISerializer<EnumComponent>
{
   public static void Serialize(ref ByteWriter writer, ref EnumComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
      NodeIdSerializer.Serialize(ref writer, ref ob.UnderlyingTypeId);
      
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.ValueIds);
   }

   public static bool TryDeserialize(ref ByteReader reader, out EnumComponent ob)
   {
      ob = new EnumComponent();
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id))
      {
         return false;
      }

      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.UnderlyingTypeId))
      {
         return false;
      }

      if (!PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.ValueIds))
      {
         return false;
      }
      
      return true;
   }
}
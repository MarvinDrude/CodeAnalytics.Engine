using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Collections;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Types;

public sealed class TypeSerializer : ISerializer<TypeComponent>
{
   public static void Serialize(ref ByteWriter writer, ref TypeComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.AttributeIds);
      
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.DirectInterfaceIds);
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.InterfaceIds);
      
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.ConstructorIds);
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.MethodIds);
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.PropertyIds);
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.FieldIds);
   }

   public static bool TryDeserialize(ref ByteReader reader, out TypeComponent ob)
   {
      ob = new TypeComponent();
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id))
      {
         return false;
      }

      if (!PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.AttributeIds))
      {
         return false;
      }
      
      if (!PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.DirectInterfaceIds)
          || !PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.InterfaceIds))
      {
         return false;
      }

      if (!PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.ConstructorIds)
          || !PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.MethodIds)
          || !PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.PropertyIds)
          || !PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.FieldIds))
      {
         return false;
      }
      
      return true;
   }
}
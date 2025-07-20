using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Collections;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Members;

public sealed class ConstructorSerializer : ISerializer<ConstructorComponent>
{
   public static void Serialize(ref ByteWriter writer, ref ConstructorComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);

      writer.WriteLittleEndian(ob.CyclomaticComplexity);
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.ParameterIds);
   }

   public static bool TryDeserialize(ref ByteReader reader, out ConstructorComponent ob)
   {
      ob = new ConstructorComponent();

      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id))
      {
         return false;
      }

      ob.CyclomaticComplexity = reader.ReadLittleEndian<int>();

      if (!PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.ParameterIds))
      {
         return false;
      }
      
      return true;
   }
}
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Collections;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Components.Members;

public sealed class MethodSerializer : ISerializer<MethodComponent>
{
   public static void Serialize(ref ByteWriter writer, ref MethodComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
      NodeIdSerializer.Serialize(ref writer, ref ob.OverrideId);
      writer.WriteByte(ob.Flags.RawByte);

      writer.WriteLittleEndian(ob.CyclomaticComplexity);
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.ParameterIds);
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.InterfaceImplementations);
   }

   public static bool TryDeserialize(ref ByteReader reader, out MethodComponent ob)
   {
      ob = new MethodComponent();
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id)
          || !NodeIdSerializer.TryDeserialize(ref reader, out ob.OverrideId))
      {
         return false;
      }

      ob.Flags = new PackedBools(reader.ReadByte());
      ob.CyclomaticComplexity = reader.ReadLittleEndian<int>();
      
      if (!PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.ParameterIds)
          || !PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.InterfaceImplementations))
      {
         return false;
      }
      
      return true;
   }
}
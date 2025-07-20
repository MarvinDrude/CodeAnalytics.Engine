using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Enums.Symbols;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Intermediate.Members;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Collections;
using CodeAnalytics.Engine.Serialization.Ids;
using CodeAnalytics.Engine.Serialization.Intermediate.Members;

namespace CodeAnalytics.Engine.Serialization.Components.Members;

public sealed class MemberSerializer : ISerializer<MemberComponent>
{
   public static void Serialize(ref ByteWriter writer, ref MemberComponent ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.Id);
      PooledSetSerializer<NodeId, NodeIdSerializer>.Serialize(ref writer, ref ob.AttributeIds);

      writer.WriteLittleEndian(ob.Access);
      writer.WriteByte(ob.Flags.RawByte);
      
      NodeIdSerializer.Serialize(ref writer, ref ob.MemberTypeId);
      NodeIdSerializer.Serialize(ref writer, ref ob.ContainingTypeId);
      
      PooledListSerializer<MemberUsageInfo, MemberUsageSerializer>.Serialize(ref writer, ref ob.InnerMemberUsages);
   }

   public static bool TryDeserialize(ref ByteReader reader, out MemberComponent ob)
   {
      ob = new MemberComponent();
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.Id))
      {
         return false;
      }

      if (!PooledSetSerializer<NodeId, NodeIdSerializer>.TryDeserialize(ref reader, out ob.AttributeIds))
      {
         return false;
      }

      ob.Access = reader.ReadLittleEndian<AccessModifier>();
      ob.Flags = new PackedBools(reader.ReadByte());

      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.MemberTypeId)
          || !NodeIdSerializer.TryDeserialize(ref reader, out ob.ContainingTypeId))
      {
         return false;
      }

      if (!PooledListSerializer<MemberUsageInfo, MemberUsageSerializer>
             .TryDeserialize(ref reader, out ob.InnerMemberUsages))
      {
         return false;
      }
      
      return true;
   }
}
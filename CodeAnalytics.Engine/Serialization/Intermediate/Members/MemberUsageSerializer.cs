using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Enums.Intermediate;
using CodeAnalytics.Engine.Contracts.Intermediate.Members;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;

namespace CodeAnalytics.Engine.Serialization.Intermediate.Members;

public sealed class MemberUsageSerializer : ISerializer<MemberUsageInfo>
{
   public static void Serialize(ref ByteWriter writer, ref MemberUsageInfo ob)
   {
      NodeIdSerializer.Serialize(ref writer, ref ob.ContainingId);
      NodeIdSerializer.Serialize(ref writer, ref ob.MemberId);

      writer.WriteLittleEndian(ob.Type);
      writer.WriteLittleEndian(ob.LoopScore);

      writer.WriteByte(ob.Flags.RawByte);
   }

   public static bool TryDeserialize(ref ByteReader reader, out MemberUsageInfo ob)
   {
      ob = new MemberUsageInfo();
      
      if (!NodeIdSerializer.TryDeserialize(ref reader, out ob.ContainingId)
          || !NodeIdSerializer.TryDeserialize(ref reader, out ob.MemberId))
      {
         return false;
      }

      ob.Type = reader.ReadLittleEndian<MemberUsageType>();
      ob.LoopScore = reader.ReadLittleEndian<int>();

      ob.Flags = new PackedBools(reader.ReadByte());
      
      return true;
   }
}
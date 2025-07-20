using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Collectors;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;
using CodeAnalytics.Engine.Serialization.System.Common;

namespace CodeAnalytics.Engine.Serialization.Stores;

public sealed class LineCountStatsSerializer : ISerializer<LineCountStats>
{
   public static void Serialize(ref ByteWriter writer, ref LineCountStats ob)
   {
      var projectId = ob.ProjectId;
      StringIdSerializer.Serialize(ref writer, ref projectId);
      
      writer.WriteLittleEndian(ob.CodeCount);
      writer.WriteLittleEndian(ob.LineCount);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out LineCountStats ob)
   {
      if (!StringIdSerializer.TryDeserialize(ref reader, out var projectId))
      {
         ob = null;
         return false;
      }
      
      ob = new LineCountStats
      {
         CodeCount = reader.ReadLittleEndian<int>(),
         LineCount = reader.ReadLittleEndian<int>(),
         ProjectId = projectId
      };

      return true;
   }
}
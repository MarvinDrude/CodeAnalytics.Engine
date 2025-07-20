using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Collectors;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;
using CodeAnalytics.Engine.Serialization.System.Collections;

namespace CodeAnalytics.Engine.Serialization.Stores;

public sealed class LineCountNodeSerializer : ISerializer<LineCountNode>
{
   public static void Serialize(ref ByteWriter writer, ref LineCountNode ob)
   {
      var dict = ob.StatsPerFile;
      DictionarySerializer<StringId, StringIdSerializer, LineCountStats, LineCountStatsSerializer>
         .Serialize(ref writer, ref dict);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out LineCountNode ob)
   {
      if (!DictionarySerializer<StringId, StringIdSerializer, LineCountStats, LineCountStatsSerializer>
             .TryDeserialize(ref reader, out var dict))
      {
         ob = null;
         return false;
      }

      ob = new LineCountNode()
      {
         StatsPerFile = dict,
      };
      
      return true;
   }
}
using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;
using CodeAnalytics.Engine.Serialization.System.Collections;

namespace CodeAnalytics.Engine.Serialization.Occurrence;

public sealed class ProjectOccurrenceSerializer : ISerializer<ProjectOccurrence>
{
   public static void Serialize(ref ByteWriter writer, ref ProjectOccurrence ob)
   {
      var pathId = ob.PathId;
      StringIdSerializer.Serialize(ref writer, ref pathId);

      var dict = ob.FileOccurrences;
      DictionarySerializer<StringId, StringIdSerializer, FileOccurrence, FileOccurrenceSerializer>
         .Serialize(ref writer, ref dict);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out ProjectOccurrence ob)
   {
      ob = null;

      if (!StringIdSerializer.TryDeserialize(ref reader, out var pathId))
      {
         return false;
      }

      if (!DictionarySerializer<StringId, StringIdSerializer, FileOccurrence, FileOccurrenceSerializer>
          .TryDeserialize(ref reader, out var dict))
      {
         return false;
      }

      ob = new ProjectOccurrence()
      {
         PathId = pathId,
         FileOccurrences = dict
      };
      
      return true;
   }
}
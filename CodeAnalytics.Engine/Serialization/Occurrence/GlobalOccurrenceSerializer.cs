using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Ids;
using CodeAnalytics.Engine.Serialization.System.Collections;

namespace CodeAnalytics.Engine.Serialization.Occurrence;

public sealed class GlobalOccurrenceSerializer : ISerializer<GlobalOccurrence>
{
   public static void Serialize(ref ByteWriter writer, ref GlobalOccurrence ob)
   {
      var dict = ob.ToDictionary();
      DictionarySerializer<StringId, StringIdSerializer, ProjectOccurrence, ProjectOccurrenceSerializer>
         .Serialize(ref writer, ref dict);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out GlobalOccurrence ob)
   {
      ob = null;

      if (!DictionarySerializer<StringId, StringIdSerializer, ProjectOccurrence, ProjectOccurrenceSerializer>
             .TryDeserialize(ref reader, out var dict))
      {
         return false;
      }

      ob = new GlobalOccurrence();

      foreach (var (key, value) in dict)
      {
         ob.ProjectOccurrences[key] = value;
      }
      
      return true;
   }
}
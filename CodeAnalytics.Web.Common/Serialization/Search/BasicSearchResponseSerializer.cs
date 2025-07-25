using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Archetypes.Common;
using CodeAnalytics.Engine.Serialization.System.Collections;
using CodeAnalytics.Engine.Serialization.System.Common;
using CodeAnalytics.Web.Common.Responses.Search;

namespace CodeAnalytics.Web.Common.Serialization.Search;

public sealed class BasicSearchResponseSerializer : ISerializer<BasicSearchResponse>
{
   public static void Serialize(ref ByteWriter writer, ref BasicSearchResponse ob)
   {
      writer.WriteLittleEndian(ob.MaxResults);

      var strings = ob.Strings;
      DictionarySerializer<int, UnmanagedSerializer<int>, string, StringSerializer>
         .Serialize(ref writer, ref strings);

      var archetypes = ob.Results;
      DynamicArchetypeSerializer.Serialize(ref writer, ref archetypes);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out BasicSearchResponse ob)
   {
      var maxResults = reader.ReadLittleEndian<int>();

      if (!DictionarySerializer<int, UnmanagedSerializer<int>, string, StringSerializer>
             .TryDeserialize(ref reader, out var dict))
      {
         ob = null;
         return false;
      }

      if (!DynamicArchetypeSerializer.TryDeserialize(ref reader, out var archetypes))
      {
         ob = null;
         return false;
      }
      
      ob = new BasicSearchResponse()
      {
         MaxResults = maxResults,
         Strings = dict,
         Results = archetypes
      };
      return true;
   }
}
using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;
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
      
      
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out BasicSearchResponse ob)
   {
      throw new NotImplementedException();
   }
}
using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Ids;
using CodeAnalytics.Engine.Serialization.System.Collections;
using CodeAnalytics.Engine.Serialization.System.Common;

namespace CodeAnalytics.Engine.Serialization.Ids;

public sealed class NodeIdStoreSerializer : ISerializer<NodeIdStore>
{
   public static void Serialize(ref ByteWriter writer, ref NodeIdStore ob)
   {
      var dict = ob.ToDictionary();
      writer.WriteLittleEndian(ob.NextId);

      var name = ob.Name;
      StringSerializer.Serialize(ref writer, ref name);
      
      DictionarySerializer<string, StringSerializer, int, UnmanagedSerializer<int>>
         .Serialize(ref writer, ref dict);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out NodeIdStore ob)
   {
      var nextId = reader.ReadLittleEndian<int>();
      
      if (!StringSerializer.TryDeserialize(ref reader, out var name))
      {
         ob = null;
         return false;
      }

      if (!DictionarySerializer<string, StringSerializer, int, UnmanagedSerializer<int>>
             .TryDeserialize(ref reader, out var dict))
      {
         ob = null;
         return false;
      }

      ob = new NodeIdStore(name, dict, nextId);
      return true;
   }
}
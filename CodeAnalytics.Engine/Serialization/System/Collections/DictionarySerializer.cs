using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.System.Collections;

public sealed class DictionarySerializer<TKey, TKeySerializer, TValue, TValueSerializer>
   : ISerializer<Dictionary<TKey,TValue>>
   where TKeySerializer : ISerializer<TKey>
   where TValueSerializer : ISerializer<TValue>
   where TKey : notnull
{
   public static void Serialize(ref ByteWriter writer, ref Dictionary<TKey, TValue> ob)
   {
      var count = ob.Count;
      writer.WriteLittleEndian(count);

      foreach (var (key, value) in ob)
      {
         var keySers = key;
         var valueSers = value;
         
         TKeySerializer.Serialize(ref writer, ref keySers);
         TValueSerializer.Serialize(ref writer, ref valueSers);
      }
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out Dictionary<TKey, TValue> ob)
   {
      Dictionary<TKey, TValue> result = [];
      var count = reader.ReadLittleEndian<int>();

      for (var e = 0; e < count; e++)
      {
         if (!TKeySerializer.TryDeserialize(ref reader, out var key)
             || !TValueSerializer.TryDeserialize(ref reader, out var value))
         {
            ob = null;
            return false;
         }

         result[key] = value;
      }

      ob = result;
      return true;
   }
}
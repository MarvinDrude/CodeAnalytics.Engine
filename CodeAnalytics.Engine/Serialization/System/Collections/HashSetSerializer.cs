using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.System.Collections;

public sealed class HashSetSerializer<T, TSerializer> : ISerializer<HashSet<T>>
   where TSerializer : ISerializer<T>
{
   public static void Serialize(ref ByteWriter writer, ref HashSet<T> ob)
   {
      writer.WriteLittleEndian(ob.Count);

      foreach (var item in ob)
      {
         var copy = item;
         TSerializer.Serialize(ref writer, ref copy);
      }
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out HashSet<T> ob)
   {
      var count = reader.ReadLittleEndian<int>();
      HashSet<T> result = [];

      for (var e = 0; e < count; e++)
      {
         if (!TSerializer.TryDeserialize(ref reader, out var value))
         {
            ob = null;
            return false;
         }

         result.Add(value);
      }

      ob = result;
      return true;
   }
}
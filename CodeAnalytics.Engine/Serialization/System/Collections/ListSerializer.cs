using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.System.Collections;

public sealed class ListSerializer<T, TSerializer> : ISerializer<List<T>>
   where TSerializer : ISerializer<T>
{
   public static void Serialize(ref ByteWriter writer, ref List<T> ob)
   {
      writer.WriteLittleEndian(ob.Count);

      if (ob.Count <= 0) return;
      
      var span = CollectionsMarshal.AsSpan(ob);
      foreach (ref var item in span)
      {
         TSerializer.Serialize(ref writer, ref item);
      }
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out List<T> ob)
   {
      var length = reader.ReadLittleEndian<int>();
      ob = new List<T>(length);
      
      for (var e = 0; e < length; e++)
      {
         if (!TSerializer.TryDeserialize(ref reader, out var item))
         {
            return false;
         }

         ob.Add(item);
      }

      return true;
   }
}
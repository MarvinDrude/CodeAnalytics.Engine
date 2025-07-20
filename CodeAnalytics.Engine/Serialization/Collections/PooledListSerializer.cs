using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.Collections;

public sealed class PooledListSerializer<T, TSerializer> : ISerializer<PooledList<T>>
   where TSerializer : ISerializer<T>
   where T : IEquatable<T>
{
   public static void Serialize(ref ByteWriter writer, ref PooledList<T> ob)
   {
      var writtenSpan = ob.WrittenSpan;
      writer.WriteLittleEndian(writtenSpan.Length);

      foreach (ref var item in ob)
      {
         TSerializer.Serialize(ref writer, ref item);
      }
   }

   public static bool TryDeserialize(ref ByteReader reader, out PooledList<T> ob)
   {
      var length = reader.ReadLittleEndian<int>();
      ob = new PooledList<T>(length);

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
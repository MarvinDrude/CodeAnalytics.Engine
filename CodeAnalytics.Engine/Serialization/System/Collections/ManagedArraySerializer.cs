using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.System.Collections;

public sealed class ManagedArraySerializer<T, TSerializer> : ISerializer<T[]>
   where TSerializer : ISerializer<T>
{
   public static void Serialize(ref ByteWriter writer, ref T[] ob)
   {
      writer.WriteLittleEndian(ob.Length);

      foreach (ref var item in ob.AsSpan())
      {
         TSerializer.Serialize(ref writer, ref item);
      }
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out T[] ob)
   {
      var length = reader.ReadLittleEndian<int>();
      ob = new T[length];

      for (var e = 0; e < length; e++)
      {
         if (!TSerializer.TryDeserialize(ref reader, out ob[e]))
         {
            return false;
         }
      }
      
      return true;
   }
}
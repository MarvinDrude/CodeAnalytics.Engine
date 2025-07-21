using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.System.Collections;

public sealed class ArraySerializer<T> : ISerializer<T[]>
   where T : unmanaged
{
   public static void Serialize(ref ByteWriter writer, ref T[] ob)
   {
      writer.WriteLittleEndian(ob.Length);

      foreach (var item in ob)
      {
         writer.WriteLittleEndian(item);
      }
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out T[] ob)
   {
      var length = reader.ReadLittleEndian<int>();
      ob = new T[length];

      for (var e = 0; e < length; e++)
      {
         ob[e] = reader.ReadLittleEndian<T>();
      }
      
      return true;
   }
}
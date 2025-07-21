using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization;

public static class Serializer<T, TSerializer>
   where TSerializer : ISerializer<T>
{
   public static Memory<byte> ToMemory(ref T ob)
   {
      var writer = new ByteWriter(stackalloc byte[512]);
      try
      {
         TSerializer.Serialize(ref writer, ref ob);
         return writer.WrittenSpan.ToArray();
      }
      finally
      {
         writer.Dispose();
      }
   }

   public static T FromMemory(scoped in ReadOnlySpan<byte> span)
   {
      var reader = new ByteReader(span);

      return TSerializer.TryDeserialize(ref reader, out var result) 
         ? result : throw new InvalidOperationException("Deserialization failed.");
   }
}
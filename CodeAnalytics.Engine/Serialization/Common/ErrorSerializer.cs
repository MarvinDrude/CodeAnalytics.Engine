using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.Common;

public sealed class ErrorSerializer<T> : ISerializer<Error<T>>
   where T : unmanaged
{
   public static void Serialize(ref ByteWriter writer, ref Error<T> ob)
   {
      writer.WriteLittleEndian(ob.Detail);
   }

   public static bool TryDeserialize(ref ByteReader reader, out Error<T> ob)
   {
      ob = new Error<T>(reader.ReadLittleEndian<T>());
      return true;
   }
}
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.Common;

public sealed class ManagedErrorSerializer<T, TSerializer> : ISerializer<Error<T>>
   where TSerializer : ISerializer<T>
{
   public static void Serialize(ref ByteWriter writer, ref Error<T> ob)
   {
      var copy = ob.Detail;
      TSerializer.Serialize(ref writer, ref copy);
   }

   public static bool TryDeserialize(ref ByteReader reader, out Error<T> ob)
   {
      if (!TSerializer.TryDeserialize(ref reader, out var detail))
      {
         ob = default;
         return false;
      }
      
      ob = new Error<T>(detail);
      return true;
   }
}
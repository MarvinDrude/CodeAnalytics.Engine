using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.Common;

public sealed class ResultSerializer<TSuccess, TSuccessSerializer, TError, TErrorSerializer> 
   : ISerializer<Result<TSuccess, TError>>
   where TSuccessSerializer : ISerializer<TSuccess>
   where TErrorSerializer : ISerializer<TError>
{
   public static void Serialize(ref ByteWriter writer, ref Result<TSuccess, TError> ob)
   {
      writer.WriteByte((byte)(ob.IsSuccess ? 1 : 0));

      if (ob is { HasValue: true })
      {
         TSuccessSerializer.Serialize(ref writer, ref ob.Success);
      }
      else
      {
         TErrorSerializer.Serialize(ref writer, ref ob.Error);
      }
   }

   public static bool TryDeserialize(ref ByteReader reader, out Result<TSuccess, TError> ob)
   {
      var isSuccess = reader.ReadByte() == 1;

      ob = isSuccess switch
      {
         true when TSuccessSerializer.TryDeserialize(ref reader, out var success) =>
            new Result<TSuccess, TError>(success),
         false when TErrorSerializer.TryDeserialize(ref reader, out var error) => 
            new Result<TSuccess, TError>(error),
         _ => default
      };

      return true;
   }
}
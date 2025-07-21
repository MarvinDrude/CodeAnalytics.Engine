using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization;
using CodeAnalytics.Engine.Serialization.Common;

namespace CodeAnalytics.Engine.Extensions.Memory;

public static class ResultExtensions
{
   public static Memory<byte> ToMemory<TSuccess, TSuccessSerializer, TError, TErrorSerializer>
      (this ref Result<TSuccess, TError> result)
      where TSuccessSerializer : ISerializer<TSuccess>
      where TErrorSerializer : ISerializer<TError>
   {
      return Serializer<
         Result<TSuccess, TError>,
         ResultSerializer<TSuccess, TSuccessSerializer, TError, TErrorSerializer>
      >.ToMemory(ref result);
   }
}
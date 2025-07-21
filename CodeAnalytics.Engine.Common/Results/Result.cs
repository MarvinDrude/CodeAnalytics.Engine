using System.Diagnostics.CodeAnalysis;

namespace CodeAnalytics.Engine.Common.Results;

public struct Result<TSuccess, TError>
{
   public bool IsSuccess;
   
   [MemberNotNullWhen(true, nameof(Success))]
   [MemberNotNullWhen(false, nameof(Error))]
   public bool HasValue => IsSuccess;

   public TSuccess? Success;
   public TError? Error;

   public Result(TSuccess success)
   {
      IsSuccess = true;
      Success = success;
   }

   public Result(TError error)
   {
      IsSuccess = false;
      Error = error;
   }

   public Result()
   {
      IsSuccess = false;
   }

   public override string ToString()
   {
      return HasValue ? $"SUCCESS: {Success.ToString()}" : $"ERROR: {Error.ToString()}";
   }
   
   public static implicit operator Result<TSuccess, TError>(TSuccess success) => new(success);
   public static implicit operator Result<TSuccess, TError>(TError error) => new(error);
}
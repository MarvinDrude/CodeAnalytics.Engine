using System.Diagnostics.CodeAnalysis;

namespace CodeAnalytics.Engine.Common.Results;

public readonly struct Result<TSuccess, TError>
{
   public readonly bool IsSuccess;
   
   [MemberNotNullWhen(true, nameof(Success))]
   [MemberNotNullWhen(false, nameof(Error))]
   public bool HasValue => IsSuccess;

   public readonly TSuccess? Success;
   public readonly TError? Error;

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

   public override string ToString()
   {
      return HasValue ? $"SUCCESS: {Success.ToString()}" : $"ERROR: {Error.ToString()}";
   }
   
   public static implicit operator Result<TSuccess, TError>(TSuccess success) => new(success);
   public static implicit operator Result<TSuccess, TError>(TError error) => new(error);
}
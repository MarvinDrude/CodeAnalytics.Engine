namespace CodeAnalytics.Engine.Common.Results;

public struct Result<TSuccess, TError>
{
   public bool IsSuccess;

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
   
   public static implicit operator Result<TSuccess, TError>(TSuccess success) => new(success);
   public static implicit operator Result<TSuccess, TError>(TError error) => new(error);
}
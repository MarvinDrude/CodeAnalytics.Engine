namespace CodeAnalytics.Engine.Common.Results.Errors;

public readonly struct Error<T1>
{
   public readonly T1 Detail;

   public Error(T1 detail)
   {
      Detail = detail;
   }
}
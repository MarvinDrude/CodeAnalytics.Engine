using System.Runtime.ExceptionServices;

namespace CodeAnalytics.Engine.Common.Extensions;

public static class TaskExtensions
{
   public static async Task WithAggregateException(this Task source)
   {
      try
      {
         await source;
      }
      catch (Exception er)
      {
         if (source.Exception is not null) ExceptionDispatchInfo.Capture(er).Throw();
         throw;
      }
   }
}
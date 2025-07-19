namespace CodeAnalytics.Engine.Common.Threading;

public sealed class ThreadTask
{
   private readonly Func<Task> _func;
   
   public ThreadTask(Func<Task> func)
   {
      _func = func;
   }

   public async Task Run(CancellationToken token = default)
   {
      var task = Task.Factory.StartNew(
         _func, token, 
         TaskCreationOptions.LongRunning,
         TaskScheduler.Default);

      await await task;
   }
}
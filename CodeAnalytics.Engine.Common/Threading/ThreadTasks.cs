namespace CodeAnalytics.Engine.Common.Threading;

public sealed class ThreadTasks : IDisposable
{
   private readonly SemaphoreSlim _semaphore;
   
   public ThreadTasks(ThreadTasksOptions options)
   {
      _semaphore = new SemaphoreSlim(
         options.MaxDegreeOfParallelism, options.MaxDegreeOfParallelism);
   }

   public async Task Run(Func<Task> task, CancellationToken ct = default)
   {
      await new ThreadTask(async () =>
      {
         try
         {
            await _semaphore.WaitAsync(ct);
            await task();
         }
         finally
         {
            _semaphore.Release();
         }
      }).Run(ct);
   }
   
   public void Dispose()
   {
      _semaphore.Dispose();
   }
}

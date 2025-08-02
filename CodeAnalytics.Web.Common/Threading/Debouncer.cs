namespace CodeAnalytics.Web.Common.Threading;

public sealed class Debouncer : IDisposable
{
   private readonly SemaphoreSlim _semaphore = new(1, 1);
   private CancellationTokenSource? _cancellationTokenSource;

   private bool _disposed;

   public async Task Debounce(TimeSpan delay, Func<CancellationToken, Task> action)
   {
      await _semaphore.WaitAsync();
      Task task;

      try
      {
         if (_cancellationTokenSource is not null)
         {
            await _cancellationTokenSource.CancelAsync();
            _cancellationTokenSource.Dispose();
         }

         _cancellationTokenSource = new CancellationTokenSource();
         var token = _cancellationTokenSource.Token;

         task = RunDebounce(delay, action, token);
      }
      finally
      {
         _semaphore.Release();
      }

      await task;
   }

   private async Task RunDebounce(TimeSpan delay, Func<CancellationToken, Task> action, CancellationToken token)
   {
      try
      {
         await Task.Delay(delay, token);
         if (!token.IsCancellationRequested)
         {
            await action(token);
         }
      }
      catch (TaskCanceledException)
      {
         // expected
      }
   }
   
   public void Cancel() => _cancellationTokenSource?.Cancel();

   public void Dispose()
   {
      if (_disposed) return;
      _disposed = true;
      
      _cancellationTokenSource?.Dispose();
      _semaphore.Dispose();
   }
}
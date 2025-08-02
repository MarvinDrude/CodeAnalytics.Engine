
namespace CodeAnalytics.Web.Common.Threading;

public sealed class Debouncer : IDisposable
{
   private readonly Lock _lock = new ();
   private CancellationTokenSource? _cancellationTokenSource;
   private Func<CancellationToken, Task>? _latestAction;

   private bool _disposed;
   private int _running;

   public void Debounce(TimeSpan delay, Func<CancellationToken, Task> action)
   {
      lock (_lock)
      {
         _cancellationTokenSource?.Cancel();
         _cancellationTokenSource?.Dispose();
         
         _cancellationTokenSource = new CancellationTokenSource();
         var token = _cancellationTokenSource.Token;

         _latestAction = action;
         
         if (Interlocked.Exchange(ref _running, 1) == 0)
         {
            _ = RunDebounce(delay, token);
         }
      }
   }

   private async Task RunDebounce(TimeSpan delay, CancellationToken token)
   {
      while (true)
      {
         try
         {
            await Task.Delay(delay, token);
         }
         catch (OperationCanceledException)
         {
            lock (_lock)
            {
               if (_disposed) return;
               token = _cancellationTokenSource?.Token ?? CancellationToken.None;
            }

            continue;
         }

         if (_latestAction is not null)
         {
            await _latestAction.Invoke(token);
         }

         if (Interlocked.CompareExchange(ref _running, 0, 1) == 1)
         {
            // finished
            return;
         }
      }
   }
   
   public void Cancel() => _cancellationTokenSource?.Cancel();

   public void Dispose()
   {
      if (_disposed) return;
      _disposed = true;
      
      _cancellationTokenSource?.Dispose();
   }
}
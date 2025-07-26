namespace CodeAnalytics.Web.Common.Threading;

public sealed class Debouncer : IDisposable
{
   private CancellationTokenSource? _cancellationTokenSource;

   public async Task Debounce(TimeSpan delay, Func<CancellationToken, Task> action)
   {
      if (_cancellationTokenSource is not null)
      {
         await _cancellationTokenSource.CancelAsync();
         _cancellationTokenSource.Dispose();
      }
      
      _cancellationTokenSource = new CancellationTokenSource();
      var token = _cancellationTokenSource.Token;

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
      _cancellationTokenSource?.Dispose();
   }
}
using System.Threading.Channels;
using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Services;
using Beskar.CodeAnalytics.Data.Metadata.Models;
using Me.Memory.Threading;

namespace Beskar.CodeAnalytics.Dashboard.Services.Database;

public sealed class DatabaseScheduler(IDatabaseProvider provider) : IDatabaseScheduler, IAsyncDisposable
{
   private readonly IDatabaseProvider _provider = provider;
   private readonly WorkPool _workPool = new(new WorkPoolOptions()
   {
      FullMode = BoundedChannelFullMode.Wait,
      MaxDegreeOfParallelism = Environment.ProcessorCount,
   });
   
   public Task<T> Schedule<T>(Func<T> action)
   {
      return _workPool.Enqueue(action);
   }

   public Task Schedule(Action action)
   {
      return _workPool.Enqueue(() =>
      {
         action();
         return true;
      });
   }

   public Task Schedule(Action<DatabaseDescriptor> action)
   {
      return _workPool.Enqueue(() =>
      {
         action(_provider.GetDescriptor());
         return true;
      });
   }

   public async ValueTask DisposeAsync()
   {
      await _workPool.DisposeAsync();
   }
}
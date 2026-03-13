using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Services;

public interface IDatabaseScheduler
{
   public Task<T> Schedule<T>(Func<T> action);
   
   public Task Schedule(Action action);

   public Task Schedule(Action<DatabaseDescriptor> action);
}
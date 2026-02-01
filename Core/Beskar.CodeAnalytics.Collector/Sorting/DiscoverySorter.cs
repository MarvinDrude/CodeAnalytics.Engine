using Beskar.CodeAnalytics.Collector.Options;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Me.Memory.Threading;

namespace Beskar.CodeAnalytics.Collector.Sorting;

public sealed class DiscoverySorter(
   WorkPool workPool, CollectorOptions collectOptions,
   DiscoveryResult result)
{
   private readonly WorkPool _workPool = workPool;
   private readonly CollectorOptions _collectOptions = collectOptions;
   
   private readonly DiscoveryResult _result = result;

   public async Task Run(CancellationToken token)
   {
      
   }
}
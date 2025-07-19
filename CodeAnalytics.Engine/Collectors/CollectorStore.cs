using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Ids;

namespace CodeAnalytics.Engine.Collectors;

public sealed class CollectorStore
{
   public required NodeIdStore NodeIdStore { get; init; }
   public required StringIdStore StringIdStore { get; init; }
   
   public required MergableComponentStore ComponentStore { get; init; }
   public required LineCountStore LineCountStore { get; init; }

   public void Merge(CollectorStore collectorStore)
   {
      ComponentStore.Merge(collectorStore.ComponentStore);
      LineCountStore.Merge(collectorStore.LineCountStore);
   }
}
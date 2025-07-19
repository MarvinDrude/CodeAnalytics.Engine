using System.Collections.Immutable;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Ids;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Common;

public sealed class AttributeCollector
{
   public static void Apply(
      ref PooledSet<NodeId> set, 
      ImmutableArray<AttributeData> attributes,
      CollectContext context)
   {
      foreach (var attribute in attributes)
      {
         if (attribute.AttributeClass?.OriginalDefinition is not { } def)
         {
            continue;
         }

         set.Add(context.Store.NodeIdStore.GetOrAdd(def));
      }
   }
}
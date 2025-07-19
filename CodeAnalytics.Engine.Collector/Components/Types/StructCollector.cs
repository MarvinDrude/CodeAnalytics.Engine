using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Types;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Types;

public sealed class StructCollector 
   : IComponentCollector<StructComponent, INamedTypeSymbol>
{
   public static bool TryParse(
      INamedTypeSymbol symbol, CollectContext context, out StructComponent component)
   {
      var store = context.Store;
      component = new StructComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol),
         IsReadOnly = symbol.IsReadOnly,
         IsRef = symbol.IsRefLikeType
      };
      
      return true;
   }
}
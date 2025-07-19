using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Types;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Types;

public sealed class InterfaceCollector 
   : IComponentCollector<InterfaceComponent, INamedTypeSymbol>
{
   public static bool TryParse(
      INamedTypeSymbol symbol, CollectContext context, out InterfaceComponent component)
   {
      var store = context.Store;
      component = new InterfaceComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol)
      };
      
      return true;
   }
}
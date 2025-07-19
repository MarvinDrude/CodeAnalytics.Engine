using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Members;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Members;

public sealed class ConstructorCollector 
   : IComponentCollector<ConstructorComponent, IMethodSymbol>
{
   public static bool TryParse(
      IMethodSymbol symbol, CollectContext context, out ConstructorComponent component)
   {
      var store = context.Store;
      component = new ConstructorComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol)
      };
      
      return true;
   }
}
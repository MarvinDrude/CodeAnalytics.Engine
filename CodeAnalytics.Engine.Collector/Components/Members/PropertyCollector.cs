using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Members;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Members;

public sealed class PropertyCollector 
   : IComponentCollector<PropertyComponent, IPropertySymbol>
{
   public static bool TryParse(
      IPropertySymbol symbol, CollectContext context, out PropertyComponent component)
   {
      var store = context.Store;
      component = new PropertyComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol),
         HasSetter = symbol.SetMethod is not null,
         HasGetter = symbol.GetMethod is not null,
      };
      
      return true;
   }
}
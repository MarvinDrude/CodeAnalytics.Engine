using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Types;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Types;

public sealed class EnumValueCollector 
   : IComponentCollector<EnumValueComponent, ISymbol>
{
   public static bool TryParse(
      ISymbol symbol, CollectContext context, out EnumValueComponent component)
   {
      var store = context.Store;
      component = new EnumValueComponent();
      
      
   }
}
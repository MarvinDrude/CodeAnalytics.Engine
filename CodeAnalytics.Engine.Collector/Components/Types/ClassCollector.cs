using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Types;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Types;

public sealed class ClassCollector 
   : IComponentCollector<ClassComponent, INamedTypeSymbol>
{
   public static bool TryParse(
      INamedTypeSymbol symbol, CollectContext context, out ClassComponent component)
   {
      var store = context.Store;
      component = new ClassComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol),
         IsAbstract = symbol.IsAbstract,
         IsStatic = symbol.IsStatic,
         IsSealed = symbol.IsSealed
      };
      
      if (symbol.BaseType is 
          { SpecialType: not SpecialType.System_Object })
      {
         component.BaseClassId = store.NodeIdStore.GetOrAdd(symbol.BaseType.OriginalDefinition);
      }
      
      return true;
   }
}
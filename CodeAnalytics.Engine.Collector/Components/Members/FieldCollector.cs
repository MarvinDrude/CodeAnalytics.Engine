using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Members;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Members;

public sealed class FieldCollector 
   : IComponentCollector<FieldComponent, IFieldSymbol>
{
   public static bool TryParse(
      IFieldSymbol symbol, CollectContext context, out FieldComponent component)
   {
      if (symbol is
          {
             IsImplicitlyDeclared: true, 
             DeclaredAccessibility: Accessibility.Private, 
             AssociatedSymbol: IPropertySymbol
          })
      {
         component = default; // auto property backing field ignored
         return false;
      }
      
      var store = context.Store;
      component = new FieldComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol),
         IsConst = symbol.IsConst,
         IsReadOnly = symbol.IsReadOnly,
      };
      
      return true;
   }
}
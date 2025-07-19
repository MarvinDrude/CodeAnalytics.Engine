using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Common;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Members;

public sealed class MemberCollector 
   : IComponentCollector<MemberComponent, ISymbol>
{
   public static bool TryParse(
      ISymbol symbol, CollectContext context, out MemberComponent component)
   {
      var store = context.Store;
      component = new MemberComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol),
         Access = symbol.DeclaredAccessibility.Map(),
         IsStatic = symbol.IsStatic,
      };

      var parent = symbol.ContainingType.OriginalDefinition;
      component.ContainingTypeId = store.NodeIdStore.GetOrAdd(parent);
      
      var memberTypeMaybe = symbol switch
      {
         IPropertySymbol property => property.Type,
         IFieldSymbol field => field.Type,
         IMethodSymbol method => method.ReturnType,
         _ => null
      };

      if (memberTypeMaybe is not null)
      {
         component.MemberTypeId = store.NodeIdStore.GetOrAdd(memberTypeMaybe.OriginalDefinition);
      }
      
      ref var attributeSet = ref component.AttributeIds;
      AttributeCollector.Apply(ref attributeSet, symbol.GetAttributes(), context);
      
      return true;
   }
}
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

      GetInnerMemberUsages(symbol, ref component);
      
      return true;
   }

   private static void GetInnerMemberUsages(ISymbol symbol, ref MemberComponent component)
   {
      switch (symbol)
      {
         case IMethodSymbol method:
            GetInnerMemberUsages(method, ref component);
            break;
         case IPropertySymbol property:
            if (property.GetMethod is not null) GetInnerMemberUsages(property.GetMethod, ref component);
            if (property.SetMethod is not null) GetInnerMemberUsages(property.SetMethod, ref component);
            break;
      }
   }

   private static void GetInnerMemberUsages(IMethodSymbol method, ref MemberComponent component)
   {
      
   }
}
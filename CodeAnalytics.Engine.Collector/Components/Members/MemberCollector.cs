using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Common;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Collector.Walkers.Members;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Enums.Symbols;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;

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

      if (!GetInnerMemberUsages(symbol, context, ref component))
      {
         component.InnerMemberUsages = [];
      }
      
      return true;
   }

   private static bool GetInnerMemberUsages(ISymbol symbol, CollectContext context, ref MemberComponent component)
   {
      switch (symbol)
      {
         case IMethodSymbol method:
            return GetInnerMemberUsages(method, context, ref component);
         case IPropertySymbol property:
            if (property.GetMethod is not null) return GetInnerMemberUsages(property.GetMethod, context, ref component);
            if (property.SetMethod is not null) return GetInnerMemberUsages(property.SetMethod, context, ref component);
            break;
      }

      return false;
   }

   private static bool GetInnerMemberUsages(IMethodSymbol method, CollectContext context, ref MemberComponent component)
   {
      foreach (var reference in method.DeclaringSyntaxReferences)
      {
         if (reference.GetSyntax() is not { } syntax
             || syntax.SyntaxTree.FilePath != context.SyntaxTree.FilePath)
         {
            continue;
         }

         var operation = context.SemanticModel.GetOperation(syntax, context.CancellationToken);
         var body = operation switch
         {
            IMethodBodyOperation mbo => mbo.BlockBody as IOperation ?? mbo.ExpressionBody,
            IConstructorBodyOperation cbo => cbo.BlockBody,
            _ => operation
         };

         if (body is null) continue;
         
         var walker = new MethodOperationWalker(context);
         walker.Visit(body);
      
         component.InnerMemberUsages = walker.MemberUsages;
         return true;
      }

      return false;
   }
}
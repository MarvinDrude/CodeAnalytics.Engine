using CodeAnalytics.Engine.Collector.Calculation;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

      if (symbol.SetMethod is { } setMethod)
      {
         component.SetterCyclomaticComplexity = CalculateComplexity(setMethod, context, ref component);
      }

      if (symbol.GetMethod is { } getMethod)
      {
         component.GetterCyclomaticComplexity = CalculateComplexity(getMethod, context, ref component);
      }
      
      return true;
   }
   
   private static int CalculateComplexity(
      IMethodSymbol symbol, 
      CollectContext context, 
      ref PropertyComponent component)
   {
      var maxCount = 0;

      foreach (var reference in symbol.DeclaringSyntaxReferences)
      {
         if (reference.GetSyntax() is not AccessorDeclarationSyntax syntax
             || syntax.SyntaxTree.FilePath != context.SyntaxTree.FilePath)
         {
            continue;
         }
         
         maxCount = Math.Max(maxCount, CyclomaticComplexityCalculator.Calculate(syntax, context.SemanticModel));
      }
      
      return maxCount;
   }
}
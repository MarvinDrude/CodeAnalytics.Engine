using CodeAnalytics.Engine.Collector.Calculation;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

      component.CyclomaticComplexity = CalculateComplexity(symbol, context, ref component);
      return true;
   }
   
   private static int CalculateComplexity(
      IMethodSymbol symbol, CollectContext context, ref ConstructorComponent component)
   {
      var maxCount = 0;

      foreach (var reference in symbol.DeclaringSyntaxReferences)
      {
         if (reference.GetSyntax() is not ConstructorDeclarationSyntax syntax
             || syntax.SyntaxTree.FilePath != context.SyntaxTree.FilePath)
         {
            continue;
         }
         
         maxCount = Math.Max(maxCount, CyclomaticComplexityCalculator.Calculate(syntax, context.SemanticModel));
      }
      
      return maxCount;
   }
}
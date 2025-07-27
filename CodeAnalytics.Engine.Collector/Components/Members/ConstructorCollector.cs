﻿using CodeAnalytics.Engine.Collector.Calculation;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Extensions.Symbols;
using CodeAnalytics.Engine.Merges.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalytics.Engine.Collector.Components.Members;

public sealed class ConstructorCollector 
   : IComponentCollector<ConstructorComponent, IMethodSymbol>
{
   public static bool TryParse(
      IMethodSymbol symbol, CollectContext context, out ConstructorComponent component)
   {
      if (symbol.IsImplicitlyDeclared)
      {
         component = default; // ignore default constructors
         return false;
      }
      
      var store = context.Store;
      component = new ConstructorComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol)
      };

      component.CyclomaticComplexity = CalculateComplexity(symbol, context, ref component);
      
      foreach (var parameter in symbol.Parameters)
      {
         var id = NodeId.Empty;

         if (context.AddSubComponentsImmediately
             && ParameterCollector.TryParse(parameter, context, out var parameterComponent))
         {
            id = parameterComponent.Id;
            
            var pool = store.ComponentStore.GetOrCreatePool<ParameterComponent, ParameterMerger>();
            pool.Add(ref parameterComponent);
         }

         if (id.IsEmpty && !context.AddSubComponentsImmediately)
         {
            id = store.NodeIdStore.GetOrAdd(parameter.GenerateParameterId(symbol));
         }

         if (!id.IsEmpty)
         {
            component.ParameterIds.Add(id);
         }
      }
      
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
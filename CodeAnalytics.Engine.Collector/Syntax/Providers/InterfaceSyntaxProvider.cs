using System.Runtime.CompilerServices;
using CodeAnalytics.Engine.Collector.Archetypes.Types;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Syntax.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalytics.Engine.Collector.Syntax.Providers;

public sealed class InterfaceSyntaxProvider : ISyntaxProvider
{
   public ISyntaxPredicator Predicator { get; } = new InterfaceDefinitionPredicator();

   public ISyntaxTransformer Transformer { get; } = new InterfaceArchetypeTransformer();
   
   public sealed class InterfaceDefinitionPredicator : ISyntaxPredicator
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool Predicate(CollectContext context)
      {
         return context.SyntaxNode is InterfaceDeclarationSyntax;
      }
   }

   public sealed class InterfaceArchetypeTransformer : ISyntaxTransformer
   {
      public void Transform(CollectContext context)
      {
         context.CancellationToken.ThrowIfCancellationRequested();
         
         if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Interface } symbol)
         {
            return;
         }

         if (InterfaceArchetypeCollector.TryParse(symbol, context, out var archetype))
         {
            InterfaceArchetypeCollector.AddArchetype(context.Store, ref archetype);
         }
      }
   }
}
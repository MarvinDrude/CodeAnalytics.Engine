using System.Runtime.CompilerServices;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Syntax.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalytics.Engine.Collector.Syntax.Providers;

public sealed class StructSyntaxProvider : ISyntaxProvider
{
   public ISyntaxPredicator Predicator { get; } = new StructDefinitionPredicator();

   public ISyntaxTransformer Transformer { get; } = new StructArchetypeTransformer();
   
   public sealed class StructDefinitionPredicator : ISyntaxPredicator
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool Predicate(CollectContext context)
      {
         return context.SyntaxNode is StructDeclarationSyntax;
      }
   }

   public sealed class StructArchetypeTransformer : ISyntaxTransformer
   {
      public void Transform(CollectContext context)
      {
         context.CancellationToken.ThrowIfCancellationRequested();
         
         if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Struct } symbol)
         {
            return;
         }

         
      }
   }
}
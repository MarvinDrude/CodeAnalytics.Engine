using System.Runtime.CompilerServices;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Syntax.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalytics.Engine.Collector.Syntax.Providers;

public class ClassSyntaxProvider : ISyntaxProvider
{
   public ISyntaxPredicator Predicator { get; } = new ClassDefinitionPredicator();

   public ISyntaxTransformer Transformer { get; } = new ClassArchetypeTransformer();
   
   public sealed class ClassDefinitionPredicator : ISyntaxPredicator
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool Predicate(CollectContext context)
      {
         return context.SyntaxNode is ClassDeclarationSyntax;
      }
   }

   public sealed class ClassArchetypeTransformer : ISyntaxTransformer
   {
      public void Transform(CollectContext context)
      {
         context.CancellationToken.ThrowIfCancellationRequested();
         
         if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Class } symbol)
         {
            return;
         }

         
      }
   }
}
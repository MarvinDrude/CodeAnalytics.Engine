﻿using System.Runtime.CompilerServices;
using CodeAnalytics.Engine.Collector.Archetypes.Types;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Syntax.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalytics.Engine.Collector.Syntax.Providers;

public sealed class EnumSyntaxProvider : ISyntaxProvider
{
   public ISyntaxPredicator Predicator { get; } = new EnumDefinitionPredicator();

   public ISyntaxTransformer Transformer { get; } = new EnumArchetypeTransformer();
   
   public sealed class EnumDefinitionPredicator : ISyntaxPredicator
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool Predicate(CollectContext context)
      {
         return context.SyntaxNode is EnumDeclarationSyntax;
      }
   }

   public sealed class EnumArchetypeTransformer : ISyntaxTransformer
   {
      public void Transform(CollectContext context)
      {
         context.CancellationToken.ThrowIfCancellationRequested();
         
         if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Enum } symbol)
         {
            return;
         }
         
         
      }
   }
}
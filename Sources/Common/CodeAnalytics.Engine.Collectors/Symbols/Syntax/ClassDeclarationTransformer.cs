using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Common;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
using CodeAnalytics.Engine.Collectors.Symbols.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalytics.Engine.Collectors.Symbols.Syntax;

public sealed class ClassDeclarationTransformer : ISyntaxTransformer<ClassDeclarationSyntax>
{
   public static async Task<bool> TryTransform(ClassDeclarationSyntax node, CollectContext context)
   {
      if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Class } symbol)
      {
         return false;
      }

      if (await SymbolCollector<INamedTypeSymbol>.Collect(symbol, context) is not { } dbSymbol)
      {
         return false;
      }

      if (await ClassSymbolCollector.Collect(symbol, context) is not { } dbClassSymbol)
      {
         return false;
      }
      
      
      
      return true;
   }
}
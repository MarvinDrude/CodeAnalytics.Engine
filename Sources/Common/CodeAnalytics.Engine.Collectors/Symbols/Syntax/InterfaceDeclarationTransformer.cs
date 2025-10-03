using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Common;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
using CodeAnalytics.Engine.Collectors.Symbols.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalytics.Engine.Collectors.Symbols.Syntax;

public sealed class InterfaceDeclarationTransformer : ISyntaxTransformer<InterfaceDeclarationSyntax>
{
   public static async Task<bool> TryTransform(InterfaceDeclarationSyntax node, CollectContext context)
   {
      if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Interface } symbol)
      {
         return false;
      }
      
      if (await SymbolCollector<INamedTypeSymbol>.Collect(symbol, context) is not { } dbSymbol)
      {
         return false;
      }

      if (await InterfaceSymbolCollector.Collect(symbol, context) is not { } dbInterfaceSymbol)
      {
         return false;
      }
      
      return true;
   }
}
using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Common;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
using CodeAnalytics.Engine.Collectors.Symbols.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalytics.Engine.Collectors.Symbols.Syntax;

public sealed class EnumDeclarationTransformer : ISyntaxTransformer<EnumDeclarationSyntax>
{
   public static async Task<bool> TryTransform(EnumDeclarationSyntax node, CollectContext context)
   {
      if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Enum } symbol)
      {
         return false;
      }
      
      if (await SymbolCollector<INamedTypeSymbol>.Collect(symbol, context) is not { } dbSymbol)
      {
         return false;
      }

      if (await EnumSymbolCollector.Collect(symbol, context) is not { } dbEnumSymbol)
      {
         return false;
      }
      
      return true;
   }
}
using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
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

      return true;
   }
}
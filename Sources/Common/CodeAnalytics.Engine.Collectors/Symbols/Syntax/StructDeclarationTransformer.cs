using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalytics.Engine.Collectors.Symbols.Syntax;

public sealed class StructDeclarationTransformer : ISyntaxTransformer<StructDeclarationSyntax>
{
   public static async Task<bool> TryTransform(StructDeclarationSyntax node, CollectContext context)
   {
      if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Struct } symbol)
      {
         return false;
      }

      
      
      return true;
   }
}
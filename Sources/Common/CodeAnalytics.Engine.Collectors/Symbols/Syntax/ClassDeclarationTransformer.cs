using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
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

      
      
      return true;
   }
}
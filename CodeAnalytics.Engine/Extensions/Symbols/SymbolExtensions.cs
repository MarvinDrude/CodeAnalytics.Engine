using System.Runtime.InteropServices;
using System.Security.Cryptography;
using CodeAnalytics.Engine.Extensions.System;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Extensions.Symbols;

public static class SymbolExtensions
{
   public static string GenerateId(this ISymbol symbol)
   {
      return symbol.GenerateSeedId().GenerateId();
   }
   
   public static string GenerateSeedId(this ISymbol symbol)
   {
      if (symbol.GetDocumentationCommentId() is not { } seed)
      {
         seed = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
      }

      if (symbol is IMethodSymbol { MethodKind: MethodKind.LocalFunction })
      {
         seed = $"LF:{seed}";
      }
      
      return seed;
   }
}
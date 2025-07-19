using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Extensions.Symbols;

public static class SymbolExtensions
{
   public static string GenerateId(this ISymbol symbol)
   {
      if (symbol.GetDocumentationCommentId() is not { } seed)
      {
         seed = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
      }
      
      var bytes = SHA256.HashData(MemoryMarshal.AsBytes(seed.AsSpan()));
      return Convert.ToHexStringLower(bytes);
   }
}
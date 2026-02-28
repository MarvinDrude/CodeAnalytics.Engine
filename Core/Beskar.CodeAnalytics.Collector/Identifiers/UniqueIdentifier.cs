using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Identifiers;

public static class UniqueIdentifier
{
   public static string? Create(ISymbol symbol)
   {
      return symbol.Kind is SymbolKind.Local or SymbolKind.Parameter 
         ? $"{Create(symbol.ContainingSymbol)}:{symbol.MetadataName}" 
         : GetDocId(symbol);
   }

   private static string? GetDocId(ISymbol symbol)
   {
      if (!symbol.IsDefinition)
      {
         symbol = symbol.OriginalDefinition;
      }

      return symbol.GetDocumentationCommentId();
   }
}
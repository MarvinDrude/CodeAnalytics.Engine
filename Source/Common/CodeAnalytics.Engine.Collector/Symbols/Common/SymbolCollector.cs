using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Extensions;
using CodeAnalytics.Engine.Collector.Formats;
using CodeAnalytics.Engine.Collector.Symbols.Interfaces;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Symbols.Common;

public sealed class SymbolCollector : ISymbolCollector<DbSymbol, ISymbol>
{
   public static async Task<DbSymbol?> Collect(ISymbol symbol, CollectContext context)
   {
      var dbSymbol = new DbSymbol()
      {
         Name = symbol.ToDisplayString(SymbolDisplayFormats.NameWithGenerics),
         MetadataName = symbol.MetadataName,
         FullPathName = GetFullPathName(symbol),
         Language = symbol.Language,
         AccessModifier = symbol.DeclaredAccessibility.ToAccessModifier(),
         Type = symbol.Kind.ToType(),
         
         CreatedAt = DateTimeOffset.UtcNow,
      };
      symbol.Get
      SymbolKey.CanCreate()
   }

   private static string GetFullPathName(ISymbol symbol)
   {
      return symbol switch
      {
         IMethodSymbol or IPropertySymbol or IFieldSymbol => 
            GetFullPathName(symbol.ContainingType.OriginalDefinition) 
            + "." + symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
         _ => symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
      };
   }
}
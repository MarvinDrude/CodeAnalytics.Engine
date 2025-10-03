using CodeAnalytics.Engine.Collectors.Extensions.Database;
using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Common;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Members;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collectors.Symbols.Members;

public sealed class FieldSymbolCollector : ISymbolCollector<DbFieldSymbol, IFieldSymbol>
{
   public static async Task<DbFieldSymbol?> Collect(IFieldSymbol symbol, CollectContext context)
   {
      var symbolId = SymbolIdentifier.Create(symbol);
      var symbolIdHash = symbolId.CreateHash();

      var symbolDatabaseId = await context.GetDbSymbolId(symbolIdHash);
      if (symbolDatabaseId == DbSymbolId.Empty) return null;
      
      
   }
}
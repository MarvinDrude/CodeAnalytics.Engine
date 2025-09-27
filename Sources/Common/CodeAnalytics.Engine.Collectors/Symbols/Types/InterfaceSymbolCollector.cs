using CodeAnalytics.Engine.Collectors.Extensions.Database;
using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Common;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
using CodeAnalytics.Engine.Storage.Extensions;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Types;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collectors.Symbols.Types;

public sealed class InterfaceSymbolCollector : ISymbolCollector<DbInterfaceSymbol, INamedTypeSymbol>
{
   public static async Task<DbInterfaceSymbol?> Collect(INamedTypeSymbol symbol, CollectContext context)
   {
      var symbolId = SymbolIdentifier.Create(symbol);
      var symbolIdHash = symbolId.CreateHash();

      var symbolDatabaseId = await context.DbContext.GetSymbolId(symbolIdHash);
      if (symbolDatabaseId == DbSymbolId.Empty) return null;
      
      return await context.DbContext.UpdateOrCreate(context.DbContext.InterfaceSymbols)
         .Match(x => x.SymbolId == symbolDatabaseId)
         .OnCreate(DbSymbolCreator)
         .Execute();
      
      DbInterfaceSymbol DbSymbolCreator() =>
         new ()
         {
            SymbolId = symbolDatabaseId,
            IsAnonymous = symbol.IsAnonymousType
         };
   }
}
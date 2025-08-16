using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Symbols.Interfaces;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Types;
using CodeAnalytics.Engine.Storage.Extensions;
using Microsoft.CodeAnalysis;
using CodeAnalytics.Engine.Collector.Extensions.Database;

namespace CodeAnalytics.Engine.Collector.Symbols.Types;

public sealed class StructSymbolCollector : ISymbolCollector<DbStructSymbol, INamedTypeSymbol>
{
   public static async Task<DbStructSymbol?> Collect(INamedTypeSymbol symbol, CollectContext context)
   {
      var symbolId = await context.DbMainContext.GetSymbolId(symbol);
      
      return await context.DbMainContext.GetOrCreate(context.DbMainContext.StructSymbols)
         .Where(x => x.SymbolId == symbolId)
         .OnCreate(DbSymbolCreator)
         .Execute();
      
      DbStructSymbol DbSymbolCreator() =>
         new ()
         {
            SymbolId = symbolId,
            IsAnonymous = symbol.IsAnonymousType,
            IsRecord = symbol.IsRecord,
            IsReadOnly = symbol.IsReadOnly,
            IsRef = symbol.IsRefLikeType
         };
   }
}
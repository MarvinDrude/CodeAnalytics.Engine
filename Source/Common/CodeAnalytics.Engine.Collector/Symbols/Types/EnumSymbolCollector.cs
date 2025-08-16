using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Extensions.Database;
using CodeAnalytics.Engine.Collector.Symbols.Common;
using CodeAnalytics.Engine.Collector.Symbols.Interfaces;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Types;
using CodeAnalytics.Engine.Storage.Extensions;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Symbols.Types;

public sealed class EnumSymbolCollector : ISymbolCollector<DbEnumSymbol, INamedTypeSymbol>
{
   public static async Task<DbEnumSymbol?> Collect(INamedTypeSymbol symbol, CollectContext context)
   {
      var symbolId = await context.DbMainContext.GetSymbolId(symbol);

      if (await SymbolCollector.Collect(symbol.EnumUnderlyingType))
      {
         
      }
      
      return await context.DbMainContext.GetOrCreate(context.DbMainContext.EnumSymbols)
         .Where(x => x.SymbolId == symbolId)
         .OnCreate(DbSymbolCreator)
         .Execute();
      
      DbEnumSymbol DbSymbolCreator() =>
         new ()
         {
            SymbolId = symbolId,
            
         };
   }
}
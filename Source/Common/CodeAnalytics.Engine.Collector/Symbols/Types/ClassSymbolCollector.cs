using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Symbols.Interfaces;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Types;
using CodeAnalytics.Engine.Storage.Extensions;
using Microsoft.CodeAnalysis;
using CodeAnalytics.Engine.Collector.Extensions.Database;
using CodeAnalytics.Engine.Collector.Symbols.Common;

namespace CodeAnalytics.Engine.Collector.Symbols.Types;

public sealed class ClassSymbolCollector : ISymbolCollector<DbClassSymbol, INamedTypeSymbol>
{
   public static async Task<DbClassSymbol?> Collect(INamedTypeSymbol symbol, CollectContext context)
   {
      var symbolId = await context.DbMainContext.GetSymbolId(symbol);
      long baseSymbolId = 0;
      
      if (symbol.BaseType is { SpecialType: not SpecialType.System_Object }
          && await SymbolCollector.Collect(symbol.BaseType.OriginalDefinition, context) is { } dbBaseSymbol)
      {
         baseSymbolId = dbBaseSymbol.Id;
      }
      
      return await context.DbMainContext.GetOrCreate(context.DbMainContext.ClassSymbols)
         .Where(x => x.SymbolId == symbolId)
         .OnCreate(DbSymbolCreator)
         .Execute();
      
      DbClassSymbol DbSymbolCreator() =>
         new ()
         {
            SymbolId = symbolId,
            BaseClassSymbolId = baseSymbolId,
            IsAnonymous = symbol.IsAnonymousType,
            IsRecord = symbol.IsRecord
         };
   }
}
using CodeAnalytics.Engine.Collectors.Extensions.Database;
using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Common;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
using CodeAnalytics.Engine.Storage.Extensions;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Types;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collectors.Symbols.Types;

public sealed class ClassSymbolCollector : ISymbolCollector<DbClassSymbol, INamedTypeSymbol>
{
   public static async Task<DbClassSymbol?> Collect(INamedTypeSymbol symbol, CollectContext context)
   {
      var symbolId = SymbolIdentifier.Create(symbol);
      var symbolIdHash = symbolId.CreateHash();

      var symbolDatabaseId = await context.DbContext.GetSymbolId(symbolIdHash);
      if (symbolDatabaseId == DbSymbolId.Empty) return null;
      
      DbSymbolId? baseSymbolId = null;
      
      if (symbol.BaseType is { SpecialType: not SpecialType.System_Object }
          && await SymbolCollector<INamedTypeSymbol>.Collect(symbol.BaseType.OriginalDefinition, context) is { } dbBaseSymbol)
      {
         baseSymbolId = dbBaseSymbol.Id;
      }
      
      return await context.DbContext.UpdateOrCreate(context.DbContext.ClassSymbols)
         .Match(x => x.SymbolId == symbolDatabaseId)
         .OnCreate(DbSymbolCreator)
         .Execute();
      
      DbClassSymbol DbSymbolCreator() =>
         new ()
         {
            SymbolId = symbolDatabaseId,
            BaseClassSymbolId = baseSymbolId,
            IsAnonymous = symbol.IsAnonymousType,
            IsRecord = symbol.IsRecord
         };
   }
}
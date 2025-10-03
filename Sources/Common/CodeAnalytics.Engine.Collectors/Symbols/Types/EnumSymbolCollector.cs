using CodeAnalytics.Engine.Collectors.Extensions.Database;
using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Common;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
using CodeAnalytics.Engine.Storage.Extensions;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Types;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collectors.Symbols.Types;

public sealed class EnumSymbolCollector : ISymbolCollector<DbEnumSymbol, INamedTypeSymbol>
{
   public static async Task<DbEnumSymbol?> Collect(INamedTypeSymbol symbol, CollectContext context)
   {
      var symbolId = SymbolIdentifier.Create(symbol);
      var symbolIdHash = symbolId.CreateHash();

      var symbolDatabaseId = await context.GetDbSymbolId(symbolIdHash);
      if (symbolDatabaseId == DbSymbolId.Empty) return null;

      if (symbol.EnumUnderlyingType is not { } underlyingSymbol || 
          await SymbolCollector<INamedTypeSymbol>.Collect(underlyingSymbol.OriginalDefinition, context) is not { } dbUnderlying)
      {
         return null;
      }
      
      return await context.DbContext.UpdateOrCreate(context.DbContext.EnumSymbols)
         .Match(x => x.SymbolId == symbolDatabaseId)
         .OnCreate(DbSymbolCreator)
         .Execute();
      
      DbEnumSymbol DbSymbolCreator() =>
         new()
         {
            SymbolId = symbolDatabaseId,
            UnderlyingTypeSymbolId = dbUnderlying.Id
         };
   }
}
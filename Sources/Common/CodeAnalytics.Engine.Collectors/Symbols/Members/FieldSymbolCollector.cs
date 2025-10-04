using CodeAnalytics.Engine.Collectors.Extensions.Database;
using CodeAnalytics.Engine.Collectors.Extensions.Symbols;
using CodeAnalytics.Engine.Collectors.Models.Contexts;
using CodeAnalytics.Engine.Collectors.Symbols.Common;
using CodeAnalytics.Engine.Collectors.Symbols.Interfaces;
using CodeAnalytics.Engine.Storage.Extensions;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Members;
using CodeAnalytics.Engine.Storage.Models.Symbols.Types;
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

      var typeId = SymbolIdentifier.Create(symbol.Type.OriginalDefinition);
      var typeIdHash = typeId.CreateHash();
      var typeDatabaseId = await context.GetDbSymbolId(typeIdHash);

      if (typeDatabaseId == DbSymbolId.Empty)
      {
         
      }
      
      return await context.DbContext.UpdateOrCreate(context.DbContext.FieldSymbols)
         .Match(x => x.SymbolId == symbolDatabaseId)
         .OnCreate(DbSymbolCreator)
         .Execute();
      
      DbFieldSymbol DbSymbolCreator() =>
         new ()
         {
            SymbolId = symbolDatabaseId,
            IsConst = symbol.IsConst,
            IsReadOnly = symbol.IsReadOnly,
            IsVolatile = symbol.IsVolatile,
            Nullability = symbol.NullableAnnotation.ToType(),
            
         };
   }
}
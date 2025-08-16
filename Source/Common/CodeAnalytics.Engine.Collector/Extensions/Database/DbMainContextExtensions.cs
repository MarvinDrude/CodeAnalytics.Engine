using CodeAnalytics.Engine.Collector.Extensions.Symbols;
using CodeAnalytics.Engine.Storage.Contexts;
using CodeAnalytics.Engine.Storage.Extensions;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Extensions.Database;

public static class DbMainContextExtensions
{
   extension(DbMainContext context)
   {
      public Task<long> GetSymbolId<TSymbol>(TSymbol symbol)
         where TSymbol : ISymbol
      {
         return context.GetSymbolId(symbol.GeneratedUniqueIdHash());
      }
   }
}
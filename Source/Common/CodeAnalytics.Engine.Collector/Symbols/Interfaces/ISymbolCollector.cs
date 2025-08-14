using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Symbols.Interfaces;

public interface ISymbolCollector<TEntity, in TSymbol>
   where TEntity : class
   where TSymbol : ISymbol
{
   public static abstract Task<DbSymbol?> Collect(TSymbol symbol, CollectContext context);
}
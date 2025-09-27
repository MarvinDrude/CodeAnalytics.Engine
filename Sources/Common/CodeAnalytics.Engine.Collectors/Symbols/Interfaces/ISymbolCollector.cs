using CodeAnalytics.Engine.Collectors.Models.Contexts;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collectors.Symbols.Interfaces;

public interface ISymbolCollector<TEntity, in TSymbol>
   where TEntity : class
   where TSymbol : ISymbol
{
   public static abstract Task<TEntity?> Collect(TSymbol symbol, CollectContext context);
}
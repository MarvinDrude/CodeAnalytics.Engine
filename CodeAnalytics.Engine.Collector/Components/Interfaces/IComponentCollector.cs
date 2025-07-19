using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Interfaces;

public interface IComponentCollector<TComponent, TSymbol>
   where TComponent : IComponent, IEquatable<TComponent>
   where TSymbol : ISymbol
{
   public static abstract bool TryParse(
      TSymbol symbol,
      CollectContext context,
      out TComponent component);
}
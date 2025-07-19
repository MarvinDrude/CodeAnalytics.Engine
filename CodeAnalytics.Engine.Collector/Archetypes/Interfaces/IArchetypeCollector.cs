using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Archetypes.Interfaces;

public interface IArchetypeCollector<TArchetype, in TSymbol>
   where TArchetype : IArchetype, IEquatable<TArchetype>
   where TSymbol : ISymbol
{
   public static abstract bool TryParse(TSymbol symbol, CollectContext context, out TArchetype archetype);
   
   public static abstract void AddArchetype(CollectorStore store, ref TArchetype archetype);
}
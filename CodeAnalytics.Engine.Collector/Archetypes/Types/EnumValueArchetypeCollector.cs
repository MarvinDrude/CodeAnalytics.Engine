using CodeAnalytics.Engine.Collector.Archetypes.Interfaces;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Common;
using CodeAnalytics.Engine.Collector.Components.Types;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Merges.Common;
using CodeAnalytics.Engine.Merges.Types;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Archetypes.Types;

public sealed class EnumValueArchetypeCollector 
   : IArchetypeCollector<EnumValueArchetype, ISymbol>
{
   public static bool TryParse(ISymbol symbol, CollectContext context, out EnumValueArchetype archetype)
   {
      if (!SymbolCollector.TryParse(symbol, context, out var symbolComp)
          || !EnumValueCollector.TryParse(symbol, context, out var typeComp))
      {
         archetype = default;
         return false;
      }

      archetype = new EnumValueArchetype()
      {
         Symbol = symbolComp,
         EnumValue = typeComp,
      };
      
      return true;
   }

   public static void AddArchetype(CollectorStore store, ref EnumValueArchetype archetype)
   {
      var symbols = store.ComponentStore.GetOrCreatePool<SymbolComponent, SymbolMerger>();
      var values = store.ComponentStore.GetOrCreatePool<EnumValueComponent, EnumValueMerger>();

      symbols.Add(ref archetype.Symbol);
      values.Add(ref archetype.EnumValue);
   }
}
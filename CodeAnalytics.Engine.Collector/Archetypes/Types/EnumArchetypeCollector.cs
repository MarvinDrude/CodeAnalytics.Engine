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

public sealed class EnumArchetypeCollector 
   : IArchetypeCollector<EnumArchetype, INamedTypeSymbol>
{
   public static bool TryParse(INamedTypeSymbol symbol, CollectContext context, out EnumArchetype archetype)
   {
      if (!SymbolCollector.TryParse(symbol, context, out var symbolComp)
          || !EnumCollector.TryParse(symbol, context, out var specificComp))
      {
         archetype = default;
         return false;
      }

      archetype = new EnumArchetype()
      {
         Symbol = symbolComp,
         Enum = specificComp
      };
      
      return true;
   }

   public static void AddArchetype(CollectorStore store, ref EnumArchetype archetype)
   {
      var symbols = store.ComponentStore.GetOrCreatePool<SymbolComponent, SymbolMerger>();
      var enums =  store.ComponentStore.GetOrCreatePool<EnumComponent, EnumMerger>();

      symbols.Add(ref archetype.Symbol);
      enums.Add(ref archetype.Enum);
   }
}
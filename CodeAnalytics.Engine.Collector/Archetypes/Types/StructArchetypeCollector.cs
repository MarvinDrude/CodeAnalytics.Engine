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

public sealed class StructArchetypeCollector : IArchetypeCollector<StructArchetype, INamedTypeSymbol>
{
   public static bool TryParse(INamedTypeSymbol symbol, CollectContext context, out StructArchetype archetype)
   {
      if (!SymbolCollector.TryParse(symbol, context, out var symbolComp)
          || !TypeCollector.TryParse(symbol, context, out var typeComp)
          || !StructCollector.TryParse(symbol, context, out var structComp))
      {
         archetype = default;
         return false;
      }

      archetype = new StructArchetype()
      {
         Symbol = symbolComp,
         Type = typeComp,
         Struct = structComp
      };
      
      return true;
   }

   public static void AddArchetype(CollectorStore store, ref StructArchetype archetype)
   {
      var symbols = store.ComponentStore.GetOrCreatePool<SymbolComponent, SymbolMerger>();
      var types = store.ComponentStore.GetOrCreatePool<TypeComponent, TypeMerger>();
      var structs = store.ComponentStore.GetOrCreatePool<StructComponent, StructMerger>();

      symbols.Add(ref archetype.Symbol);
      types.Add(ref archetype.Type);
      structs.Add(ref archetype.Struct);
   }
}
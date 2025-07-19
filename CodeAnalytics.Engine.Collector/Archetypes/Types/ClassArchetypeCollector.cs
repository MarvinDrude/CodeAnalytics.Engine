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

public sealed class ClassArchetypeCollector 
   : IArchetypeCollector<ClassArchetype, INamedTypeSymbol>
{
   public static bool TryParse(INamedTypeSymbol symbol, CollectContext context, out ClassArchetype archetype)
   {
      if (!SymbolCollector.TryParse(symbol, context, out var symbolComp)
          || !TypeCollector.TryParse(symbol, context, out var typeComp)
          || !ClassCollector.TryParse(symbol, context, out var specificComp))
      {
         archetype = default;
         return false;
      }

      archetype = new ClassArchetype()
      {
         Symbol = symbolComp,
         Type = typeComp,
         Class = specificComp
      };
      
      return true;
   }

   public static void AddArchetype(CollectorStore store, ref ClassArchetype archetype)
   {
      var symbols = store.ComponentStore.GetOrCreatePool<SymbolComponent, SymbolMerger>();
      var types = store.ComponentStore.GetOrCreatePool<TypeComponent, TypeMerger>();
      var classes = store.ComponentStore.GetOrCreatePool<ClassComponent, ClassMerger>();

      symbols.Add(ref archetype.Symbol);
      types.Add(ref archetype.Type);
      classes.Add(ref archetype.Class);
   }
}
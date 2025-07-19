using CodeAnalytics.Engine.Collector.Archetypes.Interfaces;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Common;
using CodeAnalytics.Engine.Collector.Components.Members;
using CodeAnalytics.Engine.Collectors;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Merges.Common;
using CodeAnalytics.Engine.Merges.Members;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Archetypes.Members;

public sealed class ConstructorArchetypeCollector 
   : IArchetypeCollector<ConstructorArchetype, IMethodSymbol>
{
   public static bool TryParse(IMethodSymbol symbol, CollectContext context, out ConstructorArchetype archetype)
   {
      if (!SymbolCollector.TryParse(symbol, context, out var symbolComp)
          || !MemberCollector.TryParse(symbol, context, out var memberComp)
          || !ConstructorCollector.TryParse(symbol, context, out var specificComp))
      {
         archetype = default;
         return false;
      }

      archetype = new ConstructorArchetype()
      {
         Symbol = symbolComp,
         Member = memberComp,
         Constructor = specificComp
      };
      
      return true;
   }

   public static void AddArchetype(CollectorStore store, ref ConstructorArchetype archetype)
   {
      var symbols = store.ComponentStore.GetOrCreatePool<SymbolComponent, SymbolMerger>();
      var members = store.ComponentStore.GetOrCreatePool<MemberComponent, MemberMerger>();
      var specifics = store.ComponentStore.GetOrCreatePool<ConstructorComponent, ConstructorMerger>();

      symbols.Add(ref archetype.Symbol);
      members.Add(ref archetype.Member);
      specifics.Add(ref archetype.Constructor);
   }
}
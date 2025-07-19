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

public sealed class PropertyArchetypeCollector 
   : IArchetypeCollector<PropertyArchetype, IPropertySymbol>
{
   public static bool TryParse(IPropertySymbol symbol, CollectContext context, out PropertyArchetype archetype)
   {
      if (!SymbolCollector.TryParse(symbol, context, out var symbolComp)
          || !MemberCollector.TryParse(symbol, context, out var memberComp)
          || !PropertyCollector.TryParse(symbol, context, out var specificComp))
      {
         archetype = default;
         return false;
      }

      archetype = new PropertyArchetype()
      {
         Symbol = symbolComp,
         Member = memberComp,
         Property = specificComp
      };
      
      return true;
   }

   public static void AddArchetype(CollectorStore store, ref PropertyArchetype archetype)
   {
      var symbols = store.ComponentStore.GetOrCreatePool<SymbolComponent, SymbolMerger>();
      var members = store.ComponentStore.GetOrCreatePool<MemberComponent, MemberMerger>();
      var specifics =  store.ComponentStore.GetOrCreatePool<PropertyComponent, PropertyMerger>();

      symbols.Add(ref archetype.Symbol);
      members.Add(ref archetype.Member);
      specifics.Add(ref archetype.Property);
   }
}
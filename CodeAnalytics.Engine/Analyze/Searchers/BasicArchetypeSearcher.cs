using System.Runtime.CompilerServices;
using CodeAnalytics.Engine.Contracts.Analyze.Searchers;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Components.Common;

namespace CodeAnalytics.Engine.Analyze.Searchers;

public sealed class BasicArchetypeSearcher : BaseArchetypeSearcher
{
   public BasicArchetypeSearcher(AnalyzeStore store, BaseSearchOptions options) 
      : base(store, options)
   {
   }

   protected override bool Predicate(ref ClassArchetype archetype) => Predicate(ref archetype.Symbol);
   protected override bool Predicate(ref EnumArchetype archetype) => Predicate(ref archetype.Symbol);
   protected override bool Predicate(ref EnumValueArchetype archetype) => Predicate(ref archetype.Symbol);
   protected override bool Predicate(ref InterfaceArchetype archetype) => Predicate(ref archetype.Symbol);
   protected override bool Predicate(ref StructArchetype archetype) => Predicate(ref archetype.Symbol);
   
   protected override bool Predicate(ref MethodArchetype archetype) => Predicate(ref archetype.Symbol);
   protected override bool Predicate(ref FieldArchetype archetype) => Predicate(ref archetype.Symbol);
   protected override bool Predicate(ref PropertyArchetype archetype) => Predicate(ref archetype.Symbol);
   protected override bool Predicate(ref ConstructorArchetype archetype) => Predicate(ref archetype.Symbol);
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private bool Predicate(ref SymbolComponent symbol)
   {
      return symbol.Name.ToString().Contains(
         _options.SearchText,
         _options.CaseSensitive 
            ? StringComparison.Ordinal 
            : StringComparison.OrdinalIgnoreCase);
   }
}
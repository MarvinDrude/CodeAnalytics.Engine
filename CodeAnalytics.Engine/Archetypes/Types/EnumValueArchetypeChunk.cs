using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;

namespace CodeAnalytics.Engine.Archetypes.Types;

public sealed class EnumValueArchetypeChunk 
   : ArchetypeChunk<EnumValueArchetype, SymbolComponent, EnumValueComponent>
{
   public EnumValueArchetypeChunk(
      MergableComponentStore store) : base(store, CreateArchetype)
   {
   }

   public static void CreateArchetype(
      ref SymbolComponent symbol,
      ref EnumValueComponent type,
      out EnumValueArchetype archetype)
   {
      archetype = new EnumValueArchetype()
      {
         Symbol = symbol,
         EnumValue = type,
      };
   }
}
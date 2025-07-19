using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;

namespace CodeAnalytics.Engine.Archetypes.Types;

public sealed class EnumArchetypeChunk 
   : ArchetypeChunk<EnumArchetype, SymbolComponent, EnumComponent>
{
   public EnumArchetypeChunk(
      MergableComponentStore store) : base(store, CreateArchetype)
   {
   }

   public static void CreateArchetype(
      ref SymbolComponent symbol,
      ref EnumComponent enums,
      out EnumArchetype archetype)
   {
      archetype = new EnumArchetype()
      {
         Symbol = symbol,
         Enum = enums
      };
   }
}
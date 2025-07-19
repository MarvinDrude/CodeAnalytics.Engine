using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;

namespace CodeAnalytics.Engine.Archetypes.Types;

public sealed class StructArchetypeChunk 
   : ArchetypeChunk<StructArchetype, SymbolComponent, TypeComponent, StructComponent>
{
   public StructArchetypeChunk(
      MergableComponentStore store) : base(store, CreateArchetype)
   {
   }

   public static void CreateArchetype(
      ref SymbolComponent symbol,
      ref TypeComponent type,
      ref StructComponent struc,
      out StructArchetype archetype)
   {
      archetype = new StructArchetype()
      {
         Symbol = symbol,
         Type = type,
         Struct = struc
      };
   }
}
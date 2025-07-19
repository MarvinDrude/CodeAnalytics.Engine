using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;

namespace CodeAnalytics.Engine.Archetypes.Types;

public sealed class InterfaceArchetypeChunk 
   : ArchetypeChunk<InterfaceArchetype, SymbolComponent, TypeComponent, InterfaceComponent>
{
   public InterfaceArchetypeChunk(
      MergableComponentStore store) : base(store, CreateArchetype)
   {
   }

   public static void CreateArchetype(
      ref SymbolComponent symbol,
      ref TypeComponent type,
      ref InterfaceComponent interf,
      out InterfaceArchetype archetype)
   {
      archetype = new InterfaceArchetype()
      {
         Symbol = symbol,
         Type = type,
         Interface = interf
      };
   }
}
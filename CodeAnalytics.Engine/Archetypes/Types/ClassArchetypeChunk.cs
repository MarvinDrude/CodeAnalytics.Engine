using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Types;

namespace CodeAnalytics.Engine.Archetypes.Types;

public sealed class ClassArchetypeChunk 
   : ArchetypeChunk<ClassArchetype, SymbolComponent, TypeComponent, ClassComponent>
{
   public ClassArchetypeChunk(
      MergableComponentStore store) : base(store, CreateArchetype)
   {
   }

   public static void CreateArchetype(
      ref SymbolComponent symbol,
      ref TypeComponent type,
      ref ClassComponent cls,
      out ClassArchetype archetype)
   {
      archetype = new ClassArchetype()
      {
         Symbol = symbol,
         Type = type,
         Class = cls
      };
   }
}
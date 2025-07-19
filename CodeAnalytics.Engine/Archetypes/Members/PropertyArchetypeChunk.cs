using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Members;

namespace CodeAnalytics.Engine.Archetypes.Members;

public sealed class PropertyArchetypeChunk 
   : ArchetypeChunk<PropertyArchetype, SymbolComponent, MemberComponent, PropertyComponent>
{
   public PropertyArchetypeChunk(
      MergableComponentStore store) : base(store, CreateArchetype)
   {
   }

   public static void CreateArchetype(
      ref SymbolComponent symbol,
      ref MemberComponent member,
      ref PropertyComponent prop,
      out PropertyArchetype archetype)
   {
      archetype = new PropertyArchetype()
      {
         Symbol = symbol,
         Member = member,
         Property = prop
      };
   }
}
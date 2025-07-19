using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Members;

namespace CodeAnalytics.Engine.Archetypes.Members;

public sealed class FieldArchetypeChunk 
   : ArchetypeChunk<FieldArchetype, SymbolComponent, MemberComponent, FieldComponent>
{
   public FieldArchetypeChunk(
      MergableComponentStore store) : base(store, CreateArchetype)
   {
   }

   public static void CreateArchetype(
      ref SymbolComponent symbol,
      ref MemberComponent member,
      ref FieldComponent field,
      out FieldArchetype archetype)
   {
      archetype = new FieldArchetype()
      {
         Symbol = symbol,
         Member = member,
         Field = field
      };
   }
}
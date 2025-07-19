using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Members;

namespace CodeAnalytics.Engine.Archetypes.Members;

public sealed class ConstructorArchetypeChunk 
   : ArchetypeChunk<ConstructorArchetype, SymbolComponent, MemberComponent, ConstructorComponent>
{
   public ConstructorArchetypeChunk(
      MergableComponentStore store) : base(store, CreateArchetype)
   {
   }

   public static void CreateArchetype(
      ref SymbolComponent symbol,
      ref MemberComponent member,
      ref ConstructorComponent constr,
      out ConstructorArchetype archetype)
   {
      archetype = new ConstructorArchetype()
      {
         Symbol = symbol,
         Member = member,
         Constructor = constr
      };
   }
}
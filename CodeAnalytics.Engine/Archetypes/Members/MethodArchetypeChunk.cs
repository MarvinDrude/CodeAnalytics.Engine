using CodeAnalytics.Engine.Archetypes.Common;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Members;

namespace CodeAnalytics.Engine.Archetypes.Members;

public sealed class MethodArchetypeChunk 
   : ArchetypeChunk<MethodArchetype, SymbolComponent, MemberComponent, MethodComponent>
{
   public MethodArchetypeChunk(
      MergableComponentStore store) : base(store, CreateArchetype)
   {
   }

   public static void CreateArchetype(
      ref SymbolComponent symbol,
      ref MemberComponent member,
      ref MethodComponent method,
      out MethodArchetype archetype)
   {
      archetype = new MethodArchetype()
      {
         Symbol = symbol,
         Member = member,
         Method = method
      };
   }
}
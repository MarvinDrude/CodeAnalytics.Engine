using CodeAnalytics.Engine.Archetypes.Delegates;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;

namespace CodeAnalytics.Engine.Archetypes.Common;

public class ArchetypeChunk<TArchetype, TC1, TC2> 
   : ArchetypeChunkBase<TArchetype>
   where TArchetype : IArchetype, IEquatable<TArchetype>
   where TC1 : IComponent, IEquatable<TC1>
   where TC2 : IComponent, IEquatable<TC2>
{
   public ArchetypeChunk(
      MergableComponentStore store, 
      CreateArchetype<TArchetype, TC1, TC2> createFunc)
   {
      if (store.GetPool<TC1>() is not { } compsOne
          || store.GetPool<TC2>() is not { } compsTwo)
      {
         _entries = new PooledList<TArchetype>(1);
         return;
      }

      _entries = new PooledList<TArchetype>(compsTwo.Count);
      foreach (ref var two in compsTwo.Entries)
      {
         if (!compsOne.TryGetSlot(two.NodeId, out var one))
         {
            continue;
         }

         createFunc(ref compsOne.Entries.GetByReference(one), ref two, out var archetype);
         _entries.Add(archetype);
      }
   }
}
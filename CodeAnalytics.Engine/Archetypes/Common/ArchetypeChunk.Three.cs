using CodeAnalytics.Engine.Archetypes.Delegates;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;

namespace CodeAnalytics.Engine.Archetypes.Common;

public class ArchetypeChunk<TArchetype, TC1, TC2, TC3> 
   : ArchetypeChunkBase<TArchetype>
   where TArchetype : IArchetype, IEquatable<TArchetype>
   where TC1 : IComponent, IEquatable<TC1>
   where TC2 : IComponent, IEquatable<TC2>
   where TC3 : IComponent, IEquatable<TC3>
{
   public ArchetypeChunk(
      MergableComponentStore store, 
      CreateArchetype<TArchetype, TC1, TC2, TC3> createFunc)
   {
      if (store.GetPool<TC1>() is not { } compsOne
          || store.GetPool<TC2>() is not { } compsTwo
          || store.GetPool<TC3>() is not { } compsThree)
      {
         _entries = new PooledList<TArchetype>(1);
         return;
      }

      _entries = new PooledList<TArchetype>(compsThree.Count);
      foreach (ref var three in compsThree.Entries)
      {
         if (!compsOne.TryGetSlot(three.NodeId, out var one)
             || !compsTwo.TryGetSlot(three.NodeId, out var two))
         {
            continue;
         }

         createFunc(
            ref compsOne.Entries.GetByReference(one), 
            ref compsTwo.Entries.GetByReference(two), 
            ref three, out var archetype);
         _entries.Add(archetype);
      }
   }
}
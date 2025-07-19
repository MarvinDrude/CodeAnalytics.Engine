using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Components;

public sealed class MergableComponentPool<TComponent, TMerger> 
   : ComponentPool<TComponent>, IMergableComponentPool
   where TComponent : IComponent, IEquatable<TComponent>
   where TMerger : IComponentMerger<TComponent>
{
   public MergableComponentPool(int initialCapacity) 
      : base(initialCapacity)
   {
   }

   public void Merge(IMergableComponentPool source)
   {
      Merge((MergableComponentPool<TComponent, TMerger>)source);
   }
   
   public void Merge(MergableComponentPool<TComponent, TMerger> source)
   {
      foreach (ref var component in source.Entries)
      {
         var sourceId = component.NodeId;

         if (!TryGetSlot(sourceId, out var slot))
         {
            Add(ref component);
            continue;
         }

         ref var target = ref _data.GetByReference(slot);
         TMerger.Merge(ref target, ref component);
      }
   }
}
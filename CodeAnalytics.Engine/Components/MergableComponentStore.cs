using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Components;

public sealed class MergableComponentStore : ComponentStore
{
   public MergableComponentStore(int initialCapacity) 
      : base(initialCapacity)
   {
   }

   public MergableComponentPool<TComponent, TMerger> GetOrCreatePool<TComponent, TMerger>()
      where TComponent : IComponent, IEquatable<TComponent>
      where TMerger : IComponentMerger<TComponent>
   {
      var type = typeof(TComponent);
      
      if (!_pools.TryGetValue(type, out var pool))
      {
         pool = _pools[type] = new MergableComponentPool<TComponent, TMerger>(_initialCapacity);
      }

      return (MergableComponentPool<TComponent, TMerger>)pool;
   }

   public void Merge(MergableComponentStore source)
   {
      foreach (var (type, pool) in source._pools)
      {
         if (!_pools.TryGetValue(type, out var target))
         {
            _pools[type] = source._pools[type];
            continue;
         }
         
         // ugly but cant change it
         ((IMergableComponentPool)target).Merge((IMergableComponentPool)pool);
      }
   }
}
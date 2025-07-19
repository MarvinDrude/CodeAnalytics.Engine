using CodeAnalytics.Engine.Contracts.Components.Inerfaces;

namespace CodeAnalytics.Engine.Components;

public class ComponentStore : IDisposable
{
   protected readonly Dictionary<Type, IComponentPool> _pools;
   protected readonly int _initialCapacity;

   public ComponentStore(int initialCapacity)
   {
      _pools = new Dictionary<Type, IComponentPool>();
      _initialCapacity = initialCapacity;
   }
   
   public ComponentPool<TComponent>? GetPool<TComponent>()
      where TComponent : IComponent, IEquatable<TComponent>
   {
      if (_pools.TryGetValue(typeof(TComponent), out var pool))
      {
         return (ComponentPool<TComponent>)pool;
      }

      return null;
   }

   public void Dispose()
   {
      GC.SuppressFinalize(this);
      
      foreach (var (key, pool) in _pools)
      {
         pool.Dispose();
      }
      
      _pools.Clear();
   }
}
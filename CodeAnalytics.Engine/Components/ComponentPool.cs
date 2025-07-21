using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Components;

public class ComponentPool<T> : IComponentPool
   where T : IComponent, IEquatable<T>
{
   public int Count { get; protected set; }
   public int Capacity { get; protected set; }

   public ref PooledList<T> Entries => ref _data;
   
   protected PooledList<T> _data;
   protected PooledList<NodeId> _ids;
   protected PooledList<int> _sparse;

   protected bool _trimmed;

   public ComponentPool(int initialCapacity)
   {
      if (initialCapacity <= 1)
      {
         initialCapacity = 16;
      }
      
      _data = new PooledList<T>(initialCapacity);
      _ids = new PooledList<NodeId>(initialCapacity);
      _sparse = new PooledList<int>(initialCapacity);

      _sparse.Fill(-1);
      
      Capacity = initialCapacity;
      Count = 0;
   }
   
   public Result<int, Error<string>> Add(ref T component)
   {
      if (_trimmed)
      {
         return new Error<string>("Cannot add more once the component pool has been trimmed.");
      }
      
      int nodeId = component.NodeId;

      if (nodeId < 0)
      {
         return new Error<string>("No negative node ids allowed.");
      }

      var notMax = true;
      while (nodeId >= Capacity && notMax)
      {
         notMax = Resize();
      }

      if (!notMax && nodeId >= Capacity)
      {
         return new Error<string>("Max Capacity exceeded.");
      }
      
      var prev = _sparse[nodeId];
      if (prev != -1)
      {
         _data[prev] = component;
         return prev;
      }

      var slot = Count++;

      _sparse[nodeId] = slot;
      _data[slot] = component;
      _ids[slot] = component.NodeId;

      return slot;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public bool Remove(ref T component)
   {
      return Remove(component.NodeId);
   }
   
   public bool Remove(NodeId nodeId)
   {
      if (!TryGetSlot(nodeId, out var slot))
      {
         return false;
      }

      int nodeIdIndex = nodeId;
      var last = --Count;

      if (slot != last)
      {
         var lastId = _ids[last];
         
         _data[slot] = _data[last];
         _ids[slot] = lastId;
         _sparse[lastId] = slot;
      }

      _sparse[nodeIdIndex] = -1;
      return true;
   }

   public bool TryGetSlot(NodeId nodeId, out int slot)
   {
      int nodeIdIndex = nodeId;
      if (nodeIdIndex >= Capacity || nodeIdIndex < 0 
            || nodeIdIndex >= _sparse.Count)
      {
         slot = -1;
         return false;
      }

      slot = _sparse[nodeIdIndex];
      return slot > -1 && slot < Count && _ids[slot] == nodeId;
   }
   
   private bool Resize()
   {
      var oldSize = Capacity;
      var newSize = Math.Min(oldSize * 2, MaxCapacity);
      
      Capacity = newSize;

      _data.Resize(newSize);
      _ids.Resize(newSize);
      _sparse.Resize(newSize);
      
      _sparse.Fill(-1, oldSize);

      return newSize < MaxCapacity;
   }

   public void Trim()
   {
      _trimmed = true;
      
      _data.Trim();
      _ids.Trim();
      _sparse.Trim();
   }

   public void Dispose()
   {
      GC.SuppressFinalize(this);
      
      foreach (ref var node in _data)
      {
         node.Dispose();
      }

      _data.Dispose();
      _ids.Dispose();
      _sparse.Dispose();
   }

   private const int MaxCapacity = int.MaxValue - 100_000;
}
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CodeAnalytics.Engine.Common.Buffers.Dynamic;

[StructLayout(LayoutKind.Auto)]
[CollectionBuilder(typeof(PooledSetCollectionBuilder), nameof(PooledSetCollectionBuilder.Create))]
public struct PooledSet<T> : IDisposable
   where T : IEquatable<T>
{
   public Span<T> WrittenSpan
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _list.WrittenSpan;
   }
   
   public T this[int index]
   {
      get => _list[index];
      set => _list[index] = value;
   }
   
   private PooledList<T> _list;

   public PooledSet(int initialCapacity)
   {
      _list = new PooledList<T>(initialCapacity);
   }

   public PooledSet(ReadOnlySpan<T> values)
   {
      _list = new PooledList<T>(values.Length);
      AddSpanRange(values);
   }

   public bool Add(scoped in T item)
   {
      if (_list.IndexOf(item) >= 0)
      {
         return false;
      }

      _list.Add(item);
      return true;
   }

   public void AddRange(params T[] items)
   {
      AddSpanRange(items);
   }
   
   public void AddSpanRange(scoped in ReadOnlySpan<T> span)
   {
      foreach (var item in span)
      {
         Add(item);
      }
   }
   
   public ref T GetByReference(int index)
   {
      return ref _list.GetByReference(index);
   }

   public Span<T>.Enumerator GetEnumerator() => _list.WrittenSpan.GetEnumerator();
   
   public void Dispose()
   {
      _list.Dispose();
   }   
}

public static class PooledSetCollectionBuilder
{
   public static PooledSet<T> Create<T>(ReadOnlySpan<T> values) 
      where T : IEquatable<T> => new(values);
}
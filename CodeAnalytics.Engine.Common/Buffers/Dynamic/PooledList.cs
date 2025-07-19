using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CodeAnalytics.Engine.Common.Buffers.Dynamic;

[StructLayout(LayoutKind.Auto)]
[CollectionBuilder(typeof(PooledListCollectionBuilder), nameof(PooledListCollectionBuilder.Create))]
public struct PooledList<T> : IEquatable<PooledList<T>>, IDisposable
   where T : IEquatable<T>
{
   public Span<T> WrittenSpan
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _owner.Span[.._index];
   }

   public Span<T> Span
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _owner.Span;
   }
   
   public int Count
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _index;
   }

   public bool IsDisposed => _owner.IsDisposed;

   public T this[int index]
   {
      get => _owner.Span[index];
      set {
         if (_length == _index)
         {
            var newLength = _length + 1;
            EnsureSize(newLength);
         }
         
         ref var reference = ref MemoryMarshal.GetReference(_owner.Span);
         Unsafe.Add(ref reference, index) = value;

         if (index > _index)
         {
            _index = index;
         }
      }
   }
   
   private MemoryOwner<T> _owner;
   
   private int _length;
   private int _index;

   public PooledList(int initialCapacity)
   {
      _owner = MemoryAllocator<T>.CreatePooled(initialCapacity);
      _length = _owner.Length;
      _index = 0;
   }

   public PooledList(ReadOnlySpan<T> values)
   {
      _owner = MemoryAllocator<T>.CreatePooled(values.Length);
      _length = _owner.Length;

      _index = values.Length;
      values.CopyTo(_owner.Span);
   }

   public void Add(scoped in T item)
   {
      if (_length == _index)
      {
         var newLength = _length + 1;
         EnsureSize(newLength);
      }

      ref var reference = ref MemoryMarshal.GetReference(_owner.Span);
      Unsafe.Add(ref reference, _index++) = item;
   }

   public int IndexOf(T item)
   {
      return WrittenSpan.IndexOf(item);
   }

   public readonly ref T GetByReference(int index)
   {
      ref var reference = ref MemoryMarshal.GetReference(_owner.Span);
      return ref Unsafe.Add(ref reference, index);
   }

   public bool Contains(T item)
   {
      return WrittenSpan.Contains(item);
   }

   public void Resize(int newCapacity)
   {
      EnsureSize(newCapacity);
   }

   public void Fill(ReadOnlySpan<T> values)
   {
      values.CopyTo(_owner.Span);
      _index = values.Length;
      _length = values.Length;
   }

   public void Fill(T item)
   {
      _owner.Span.Fill(item);
   }

   public void Fill(T item, int startIndex)
   {
      _owner.Span[startIndex..].Fill(item);
   }

   private void EnsureSize(int size)
   {
      if (size <= _owner.Length 
          || _owner.TryResize(size))
      {
         _length = size;
         return;
      }

      var lastOwner = _owner;
      _owner = MemoryAllocator<T>.CreatePooled((int)Math.Min((long)size * size, int.MaxValue));

      lastOwner.Span.CopyTo(_owner.Span);
      lastOwner.Dispose();

      _length = _owner.Length;
   }
   
   public Span<T>.Enumerator GetEnumerator() => WrittenSpan.GetEnumerator();

   public void Dispose()
   {
      _owner.Dispose();
   }

   public bool Equals(PooledList<T> other)
   {
      return Equals(this, other);
   }

   public override bool Equals(object? obj)
   {
      return obj is PooledList<T> other && Equals(other);
   }

   public override int GetHashCode()
   {
      return base.GetHashCode();
   }

    public static bool operator ==(PooledList<T> left, PooledList<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PooledList<T> left, PooledList<T> right)
    {
        return !(left == right);
    }
}

public static class PooledListCollectionBuilder
{
   public static PooledList<T> Create<T>(ReadOnlySpan<T> values) 
      where T : IEquatable<T> => new(values);
}
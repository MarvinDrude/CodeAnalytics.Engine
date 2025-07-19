using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CodeAnalytics.Engine.Common.Buffers;

[StructLayout(LayoutKind.Auto)]
public struct MemoryOwner<T> : IDisposable
{
   public Span<T> Span
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _owner.AsSpan(0, _length);
   }

   public Memory<T> Memory
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _owner.AsMemory(0, _length);
   }

   public int Length
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _length;
   }

   public int Capacity
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _owner.Length;
   }

   public bool IsEmpty => Span.IsEmpty;
   public bool IsDisposed => _disposed;
   
   private T[] _owner;
   private readonly ArrayPool<T>? _pool;

   private int _length;
   private bool _disposed;

   public MemoryOwner(T[] array, int length)
   {
      _owner = array;
      _pool = null;
      
      _length = length;
   }

   public MemoryOwner(ArrayPool<T> pool, T[] array, int length)
   {
      _owner = array;
      _pool = pool;
      
      _length = length;
   }

   public MemoryOwner(ArrayPool<T> pool, int length)
   {
      _owner = pool.Rent(length);
      _pool = pool;

      _length = length;
   }

   public bool TryResize(int newLength)
   {
      if (newLength > Capacity)
      {
         return false;
      }
      
      _length = newLength;
      return true;
   }

   public void Dispose()
   {
      if (_disposed)
      {
         return;
      }
      
      _disposed = true;
      
      _pool?.Return(_owner);
      _owner = null!;
   }
}
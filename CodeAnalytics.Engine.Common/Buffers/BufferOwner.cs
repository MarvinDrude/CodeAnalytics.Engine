using System.Buffers;
using System.Runtime.InteropServices;

namespace CodeAnalytics.Engine.Common.Buffers;

[StructLayout(LayoutKind.Auto)]
public readonly ref struct BufferOwner<T> : IDisposable
{
   public Span<T> Span => _buffer;
   public bool IsFromPool => _pool is not null;

   public bool IsEmpty => _buffer.IsEmpty;
   public int Length => _buffer.Length;
   
   private readonly Span<T> _buffer;
   private readonly ArrayPool<T>? _pool;
   private readonly T[]? _owner;

   public BufferOwner(Span<T> buffer)
   {
      _buffer = buffer;
      _pool = null;
   }

   public BufferOwner(Span<T> buffer, int length)
   {
      _buffer = buffer[..length];
      _pool = null;
   }

   public BufferOwner(int minLength, bool exactLength = true)
      : this(ArrayPool<T>.Shared, minLength, exactLength) { }

   public BufferOwner(ArrayPool<T> pool, int minLength, bool exactLength = true)
   {
      var owner = pool.Rent(minLength);
      _buffer = exactLength 
         ? owner.AsSpan(0, minLength) 
         : owner.AsSpan();

      _owner = owner;
   }
   
   public ref T this[int index] => ref _buffer[index];
   
   public void Dispose()
   {
      if (_owner is not null)
      {
         _pool?.Return(_owner);
      }
   }
}
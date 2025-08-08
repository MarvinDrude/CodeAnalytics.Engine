using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CodeAnalytics.Engine.Common.Buffers;

[StructLayout(LayoutKind.Auto)]
public ref struct BufferWriter<T> : IDisposable
{
   public readonly int Capacity
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _owner.Length;
   }
   
   public readonly int FreeCapacity
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _owner.Length - _position;
   }

   public readonly ReadOnlySpan<T> WrittenSpan
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => Buffer[.._position];
   }
   public readonly BufferOwner<T> Owner
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _owner;
   }
   
   public int Position
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      readonly get => _position;
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      set
      {
         if (value > Capacity || value < 0)
         {
            throw new ArgumentOutOfRangeException(nameof(Position));
         }
         _position = value;
      }
   }

   private readonly Span<T> Buffer
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _owner.Span;
   }
   
   private BufferOwner<T> _owner;
   private bool _isGrown;
   private bool _disposed;

   private readonly int _initialMinGrowCapacity;
   private int _position;
   
   public BufferWriter(Span<T> startBuffer, int initialMinGrowCapacity = -1)
   {
      _owner = BufferAllocator<T>.Create(startBuffer);
      _initialMinGrowCapacity = initialMinGrowCapacity;
      
      _isGrown = false;
      _disposed = false;
      _position = 0;
   }

   public BufferWriter(BufferOwner<T> owner, int initialMinGrowCapacity = -1)
   {
      _owner = owner;
      _initialMinGrowCapacity = initialMinGrowCapacity;
      
      _isGrown = false;
      _disposed = false;
      _position = 0;
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void Fill(T value)
   {
      _owner.Span.Fill(value);
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void Add(ref T reference)
   {
      if (_owner.Length - _position < 1)
      {
         ResizeSpan(1);
      }

      ref var refr = ref MemoryMarshal.GetReference(_owner.Span);
      Unsafe.Add(ref refr, _position++) = reference;
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void Add(T value)
   {
      if (_owner.Length - _position < 1)
      {
         ResizeSpan(1);
      }

      ref var reference = ref MemoryMarshal.GetReference(_owner.Span);
      Unsafe.Add(ref reference, _position++) = value;
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void Write(scoped ReadOnlySpan<T> span)
   {
      if (_owner.Length - _position < span.Length)
      {
         ResizeSpan(span.Length - FreeCapacity);
      }
      
      ref var srcBase = ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(span));
      ref var destBase = ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(_owner.Span));

      var sizeOf = Unsafe.SizeOf<T>();
      var byteCount = (uint)(span.Length * sizeOf);
      
      // slightly faster than _owner.Span[_position..] + CopyTo 
      Unsafe.CopyBlockUnaligned(
         ref Unsafe.AddByteOffset(ref destBase, (nint)(_position * sizeOf)),
         ref srcBase,
         byteCount);

      _position += span.Length;
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public Span<T> AcquireSpan(int length, bool movePosition = true)
   {
      var start = _position;
      
      if (_owner.Length - start < length)
      {
         ResizeSpan(length - FreeCapacity);
      }
      
      if (movePosition)
      {
         _position += length;
      }

      return _owner.Span.Slice(start, length);
   }
   
   public void Move(int fromStart, int fromLength, int toStart, bool movePosition = true)
   {
      if (fromLength == 0 || fromStart == toStart)
      {
         if (toStart > _owner.Span.Length)
         {
            ResizeSpan(toStart - _owner.Span.Length);
         }
         
         return;
      }

      var oldPosition = _position;
      var newPosition = toStart + fromLength;
      var span = _owner.Span;
      
      if (newPosition > span.Length)
      {
         ResizeSpan(newPosition - span.Length);
         span = _owner.Span;
      }

      span.Slice(fromStart, fromLength)
         .CopyTo(span.Slice(toStart, fromLength));
      
      _position = movePosition ? newPosition : oldPosition;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void Advance(int count)
   {
      if (_owner.Length - _position < count)
      {
         ResizeSpan(count - FreeCapacity);
      }
      
      _position += count;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void AdvanceTo(int position)
   {
      if (_owner.Length < position)
      {
         ResizeSpan(position - _owner.Length);
      }
      
      _position = position;
   }
   
   private void ResizeSpan(int requestedLength)
   {
      int newLength;
      if (!_isGrown && _initialMinGrowCapacity >= requestedLength)
      {
         newLength = _owner.Length + _initialMinGrowCapacity;
      }
      else
      {
         var growBy = _owner.Length > 0 ? Math.Max(requestedLength, _owner.Length) : 256;
         var newLengthLong = (long)_owner.Length + growBy;
         
         newLength = (int)Math.Min(int.MaxValue - 1, newLengthLong);
      }

      var lastOwner = _owner;
      _owner = BufferAllocator<T>.CreatePooled(newLength, true);
      
      lastOwner.Span.CopyTo(_owner.Span);
      lastOwner.Dispose();
      
      _isGrown = true;
   }
   
   public void Dispose()
   {
      if (_disposed)
      {
         return;
      }
      
      _disposed = true;
      _owner.Dispose();
   }
}
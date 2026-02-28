using System.Buffers.Binary;
using System.IO.MemoryMappedFiles;
using Microsoft.Win32.SafeHandles;

namespace Beskar.CodeAnalytics.Data.Files;

public readonly unsafe ref struct MmfBuffer : IDisposable
{
   private readonly SafeMemoryMappedViewHandle _handle;
   private readonly byte* _basePointer;
   private readonly long _capacity;

   public MmfBuffer(MemoryMappedViewAccessor accessor)
   {
      _handle = accessor.SafeMemoryMappedViewHandle;
      _capacity = accessor.Capacity;
      
      byte* ptr = null;
      _handle.AcquirePointer(ref ptr);
      _basePointer = ptr + accessor.PointerOffset;
   }

   public ref T GetRef<T>(long byteOffset) where T : unmanaged
   {
      if (byteOffset + sizeof(T) > _capacity)
         throw new ArgumentOutOfRangeException(nameof(byteOffset));

      return ref *(T*)(_basePointer + byteOffset);
   }

   public Span<T> GetSpan<T>(long byteOffset, int count) where T : unmanaged
   {
      return byteOffset + ((long)count * sizeof(T)) > _capacity 
         ? throw new ArgumentOutOfRangeException(nameof(byteOffset)) 
         : new Span<T>(_basePointer + byteOffset, count);
   }

   public long ReadInt64BigEndian(long byteOffset)
   {
      ReadOnlySpan<byte> span = new(_basePointer + byteOffset, sizeof(long));
      return BinaryPrimitives.ReadInt64BigEndian(span);
   }

   public int ReadInt32BigEndian(long byteOffset)
   {
      ReadOnlySpan<byte> span = new(_basePointer + byteOffset, sizeof(int));
      return BinaryPrimitives.ReadInt32BigEndian(span);
   }

   public short ReadInt16BigEndian(long byteOffset)
   {
      ReadOnlySpan<byte> span = new(_basePointer + byteOffset, sizeof(short));
      return BinaryPrimitives.ReadInt16BigEndian(span);
   }
   
   public long ReadInt64LittleEndian(long byteOffset)
   {
      ReadOnlySpan<byte> span = new(_basePointer + byteOffset, sizeof(long));
      return BinaryPrimitives.ReadInt64LittleEndian(span);
   }

   public int ReadInt32LittleEndian(long byteOffset)
   {
      ReadOnlySpan<byte> span = new(_basePointer + byteOffset, sizeof(int));
      return BinaryPrimitives.ReadInt32LittleEndian(span);
   }

   public short ReadInt16LittleEndian(long byteOffset)
   {
      ReadOnlySpan<byte> span = new(_basePointer + byteOffset, sizeof(short));
      return BinaryPrimitives.ReadInt16LittleEndian(span);
   }

   public void Dispose()
   {
      _handle.ReleasePointer();
   }
}
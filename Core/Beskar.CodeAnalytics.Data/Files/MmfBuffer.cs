using System.Buffers.Binary;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Text;
using Me.Memory.Buffers;
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

   public Span<T> GetSpanByByteCount<T>(long byteOffset, long byteCount) where T : unmanaged
   {
      return byteOffset + byteCount > _capacity 
         ? throw new ArgumentOutOfRangeException(nameof(byteOffset)) 
         : new Span<T>(_basePointer + byteOffset, (int)(byteCount / Unsafe.SizeOf<T>()));
   }

   public string GetString(long byteOffset, int byteLength, Encoding? encoding = null)
   {
      if (byteOffset + byteLength > _capacity)
      {
         throw new ArgumentOutOfRangeException(nameof(byteOffset));
      }
      
      encoding ??= Encoding.UTF8;
      ReadOnlySpan<byte> span = new(_basePointer + byteOffset, byteLength);
      
      return encoding.GetString(span);
   }

   public ArrayBuilderResult<string> GetStrings(ReadOnlySpan<(long Offset, int Length)> descriptors,
      Encoding? encoding = null)
   {
      encoding ??= Encoding.UTF8;
      var builder = new ArrayBuilder<string>(descriptors.Length);
      
      try
      {
         foreach (var descriptor in descriptors)
         {
            ReadOnlySpan<byte> span = new(_basePointer + descriptor.Offset, descriptor.Length);
            builder.Add(encoding.GetString(span));
         }
      }
      catch (Exception)
      {
         builder.Dispose();
         throw;
      }
      
      return builder;
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
   
   public ulong ReadUInt64LittleEndian(long byteOffset)
   {
      ReadOnlySpan<byte> span = new(_basePointer + byteOffset, sizeof(long));
      return BinaryPrimitives.ReadUInt64LittleEndian(span);
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
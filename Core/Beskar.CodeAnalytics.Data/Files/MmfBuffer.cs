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

   public void Dispose()
   {
      _handle.ReleasePointer();
   }
}
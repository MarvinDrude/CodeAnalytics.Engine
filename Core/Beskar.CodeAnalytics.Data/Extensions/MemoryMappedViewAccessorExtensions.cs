using System.IO.MemoryMappedFiles;

namespace Beskar.CodeAnalytics.Data.Extensions;

public static class MemoryMappedViewAccessorExtensions
{
   extension(MemoryMappedViewAccessor accessor)
   {
      public unsafe Span<T> AcquireSpan<T>(long offset, int length)
         where T : unmanaged
      {
         byte* pointer = null;
         accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);

         try
         {
            var byteLength = (long)length * sizeof(T);
            
            if (offset < 0 || byteLength < 0 || (offset + byteLength) > accessor.Capacity)
            {
               throw new ArgumentOutOfRangeException(nameof(length), "The requested range exceeds the accessor capacity.");
            }

            var startAddress = pointer + accessor.PointerOffset + offset;
            return new Span<T>(startAddress, length);
         }
         finally
         {
            accessor.SafeMemoryMappedViewHandle.ReleasePointer();
         }
      }

      public unsafe void WriteSpan<T>(long offset, scoped in ReadOnlySpan<T> source)
         where T : unmanaged
      {
         byte* pointer = null;
         accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);

         try
         {
            var destPtr = pointer + accessor.PointerOffset + offset;
      
            var maxElements = (int)((accessor.Capacity - offset) / sizeof(T));
            var destSpan = new Span<T>(destPtr, maxElements);

            source.CopyTo(destSpan);
         }
         finally
         {
            accessor.SafeMemoryMappedViewHandle.ReleasePointer();
         }
      }
   }  
}
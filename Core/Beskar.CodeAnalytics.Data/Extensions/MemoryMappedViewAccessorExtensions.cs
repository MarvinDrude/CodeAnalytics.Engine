using System.IO.MemoryMappedFiles;
using Microsoft.Win32.SafeHandles;

namespace Beskar.CodeAnalytics.Data.Extensions;

public static class MemoryMappedViewAccessorExtensions
{
   extension(MemoryMappedViewAccessor accessor)
   {
      public MemoryMappedSpan<T> AcquireSpan<T>(long offset, int length)
         where T : unmanaged
      {
         return new MemoryMappedSpan<T>(accessor.SafeMemoryMappedViewHandle, accessor.PointerOffset + offset, length);
      }

      public unsafe void WriteSpan<T>(long offset, scoped in ReadOnlySpan<T> source)
         where T : unmanaged
      {
         byte* pointer = null;
         accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);

         try
         {
            var destPtr = pointer + accessor.PointerOffset + offset;
            var destSpan = new Span<T>(destPtr, source.Length);

            source.CopyTo(destSpan);
         }
         finally
         {
            accessor.SafeMemoryMappedViewHandle.ReleasePointer();
         }
      }
   }  
}

public readonly unsafe ref struct MemoryMappedSpan<T> 
   where T : unmanaged
{
   private readonly SafeMemoryMappedViewHandle _handle;
   public readonly Span<T> Span;

   public MemoryMappedSpan(SafeMemoryMappedViewHandle handle, long offset, int length)
   {
      _handle = handle;
      byte* ptr = null;
      _handle.AcquirePointer(ref ptr);
      
      Span = new Span<T>(ptr + offset, length);
   }

   public void Dispose()
   {
      _handle.ReleasePointer();
   }
}
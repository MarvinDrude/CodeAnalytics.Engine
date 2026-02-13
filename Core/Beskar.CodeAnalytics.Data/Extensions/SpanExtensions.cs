using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.Data.Extensions;

public static class SpanExtensions
{
   extension(scoped in Span<byte> buffer)
   {
      public T AsStruct<T>()
         where T : unmanaged
      {
         return buffer.Length < Unsafe.SizeOf<T>() 
            ? throw new InvalidOperationException("Buffer is too small for the requested type.") 
            : MemoryMarshal.Read<T>(buffer);
      }
   }
}
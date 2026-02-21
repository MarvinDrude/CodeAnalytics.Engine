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

      public Span<T> AsStructSpan<T>()
         where T : unmanaged
      {
         return MemoryMarshal.Cast<byte, T>(buffer);
      }
   }

   extension<T>(scoped in Span<T> buffer)
      where T : IComparable<T>
   {
      /// <summary>
      /// Requires both to be sorted
      /// </summary>
      public int IntersectInPlace(scoped in ReadOnlySpan<T> span)
      {
         var wIdx = 0;
         var nIdx = 0;
         var writeIdx = 0;

         while (wIdx < buffer.Length && nIdx < span.Length)
         {
            var comparison = buffer[wIdx].CompareTo(span[nIdx]);
            
            if (comparison == 0)
            {
               buffer[writeIdx++] = buffer[wIdx];
               wIdx++;
               nIdx++;
            }
            else if (comparison < 0)
            {
               wIdx++;
            }
            else
            {
               nIdx++;
            }
         }
         return writeIdx;
      }
   }
}
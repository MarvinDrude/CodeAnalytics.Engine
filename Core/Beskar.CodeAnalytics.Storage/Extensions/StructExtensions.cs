using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.Storage.Extensions;

public static class StructExtensions
{
   extension<T>(ref T str)
      where T : unmanaged
   {
      public ReadOnlySpan<byte> AsBytes()
      {
         var span = MemoryMarshal.CreateReadOnlySpan(ref str, 1);
         return MemoryMarshal.AsBytes(span);
      }

      public static T FromBytes(scoped in ReadOnlySpan<byte> bytes)
      {
         return bytes.Length < Unsafe.SizeOf<T>() 
            ? throw new InvalidOperationException("Buffer is too small for the requested type.") 
            : MemoryMarshal.Read<T>(bytes);
      }
   }
}
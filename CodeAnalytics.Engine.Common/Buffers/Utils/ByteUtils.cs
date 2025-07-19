using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CodeAnalytics.Engine.Common.Buffers.Utils;

public static class ByteUtils
{
   public static int WriteBigEndian<T>(this T value, scoped Span<byte> buffer)
      where T : unmanaged
   {
      var size = Unsafe.SizeOf<T>();
      buffer = buffer[..size];
      
      MemoryMarshal.Write(buffer, in value);
      if (BitConverter.IsLittleEndian)
      {
         buffer.Reverse();
      }

      return size;
   }

   public static int WriteLittleEndian<T>(this T value, scoped Span<byte> buffer)
      where T : unmanaged
   {
      var size = Unsafe.SizeOf<T>();
      buffer = buffer[..size];
      
      MemoryMarshal.Write(buffer, in value);
      
      if (!BitConverter.IsLittleEndian)
      {
         buffer.Reverse();
      }

      return size;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static T ReadBigEndian<T>(this scoped in Span<byte> buffer, out int read)
      where T : unmanaged => ReadBigEndian<T>((ReadOnlySpan<byte>)buffer, out read);
   
   public static T ReadBigEndian<T>(this scoped ReadOnlySpan<byte> buffer, out int read)
      where T : unmanaged
   {
      var size = Unsafe.SizeOf<T>();
      buffer = buffer[..size];
      read = size;

      if (!BitConverter.IsLittleEndian)
         return MemoryMarshal.Read<T>(buffer);
      
      Span<byte> temp = stackalloc byte[size];

      for (var e = 0; e < size; e++)
      {
         temp[e] = buffer[size - 1 - e];
      }
         
      return MemoryMarshal.Read<T>(temp);
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static T ReadLittleEndian<T>(this scoped in Span<byte> buffer, out int read)
      where T : unmanaged => ReadLittleEndian<T>((ReadOnlySpan<byte>)buffer, out read);
   
   public static T ReadLittleEndian<T>(this scoped ReadOnlySpan<byte> buffer, out int read)
      where T : unmanaged
   {
      var size = Unsafe.SizeOf<T>();
      buffer = buffer[..size];
      read = size;

      if (BitConverter.IsLittleEndian)
         return MemoryMarshal.Read<T>(buffer);
      
      Span<byte> temp = stackalloc byte[size];

      for (var e = 0; e < size; e++)
      {
         temp[e] = buffer[size - 1 - e];
      }
         
      return MemoryMarshal.Read<T>(temp);
   }
}
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace CodeAnalytics.Engine.Common.Buffers;

[StructLayout(LayoutKind.Sequential)]
public ref struct ByteReader
{
   public int BytesLeft
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _buffer.Length - _position;
   }
   
   public int Position
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _position;
      set => _position = value;
   }
   
   private readonly ReadOnlySpan<byte> _buffer;
   private int _position;

   public ByteReader(ReadOnlySpan<byte> buffer)
   {
      _buffer = buffer;
      _position = 0;
   }

   public T ReadLittleEndian<T>()
      where T : unmanaged
   {
      var size = Unsafe.SizeOf<T>();
      var span = _buffer.Slice(_position, size);
      _position += size;

      if (BitConverter.IsLittleEndian) 
         return MemoryMarshal.Read<T>(span);
      
      Span<byte> temp = stackalloc byte[size];

      for (var e = 0; e < size; e++)
      {
         temp[e] = span[size - 1 - e];
      }
         
      return MemoryMarshal.Read<T>(temp);

   }

   public T ReadBigEndian<T>()
      where T : unmanaged
   {
      var size = Unsafe.SizeOf<T>();
      var span = _buffer.Slice(_position, size);
      _position += size;
      
      if (!BitConverter.IsLittleEndian) 
         return MemoryMarshal.Read<T>(span);
      
      Span<byte> temp = stackalloc byte[size];

      for (var e = 0; e < size; e++)
      {
         temp[e] = span[size - 1 - e];
      }
         
      return MemoryMarshal.Read<T>(temp);
   }

   /// <summary>
   /// Only meant for intra process / memory to memory since it's just a recast.
   /// Same endianness required too.
   /// </summary>
   public string ReadStringRawToString(int size)
   {
      var chars = ReadStringRaw(size);
      return new string(chars);
   }
   
   /// <summary>
   /// Only meant for intra process / memory to memory since it's just a recast.
   /// Same endianness required too. (this is not a copy and is based on the underlying bytes)
   /// </summary>
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public ReadOnlySpan<char> ReadStringRaw(int size)
   {
      if (size == 0)
      {
         return [];
      }
      
      var raw = _buffer.Slice(_position, size);
      var chars = MemoryMarshal.Cast<byte, char>(raw);

      _position += size;
      return chars;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public string ReadString(int size)
   {
      return ReadString(size, Encoding.UTF8);
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public string ReadString(int size, Encoding encoding)
   {
      var str = encoding.GetString(_buffer.Slice(_position, size));
      _position += size;

      return str;
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public ReadOnlySpan<byte> ReadBytes(int length)
   {
      var span = _buffer.Slice(_position, length);
      _position += length;

      return span;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public byte ReadByte()
   {
      return _buffer[_position++];
   }
}
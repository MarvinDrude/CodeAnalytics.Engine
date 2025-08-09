using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CodeAnalytics.Engine.Common.Buffers;

[StructLayout(LayoutKind.Auto)]
public ref struct StreamReaderSlim : IDisposable
{
   public int BytesAvailable
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _end - _start;
   }
   
   private readonly Stream _stream;
   private readonly byte[] _buffer;

   private int _start; // incl
   private int _end; // excl

   public StreamReaderSlim(
      Stream stream,
      int chunkSize = 2024 * 2024)
   {
      _stream = stream;
      _buffer = ArrayPool<byte>.Shared.Rent(chunkSize);
   }

   public ReadOnlySpan<byte> AcquireSpan(int length)
   {
      Ensure(length);
      
      var span = _buffer.AsSpan(_start, length);
      _start += length;
      
      return span;
   }

   public byte ReadByte()
   {
      Ensure(1);
      
      var value = _buffer[_start];
      _start++;

      return value;
   }
   
   public void Ensure(int needed)
   {
      if (needed <= BytesAvailable) return;
      ArgumentOutOfRangeException.ThrowIfGreaterThan(needed, _buffer.Length);

      if (_start > 0 && BytesAvailable > 0)
      {
         var len = BytesAvailable;
         _buffer.AsSpan(_start, len).CopyTo(_buffer);
         _end = len;
         _start = 0;
      }
      else if (BytesAvailable == 0)
      {
         _start = _end = 0;
      }

      while (BytesAvailable < needed)
      {
         var read = _stream.Read(_buffer, _end, _buffer.Length - _end);
         if (read <= 0) throw new EndOfStreamException();
         _end += read;
      }
   }
   
   public void Dispose()
   {
      ArrayPool<byte>.Shared.Return(_buffer);
   }
}
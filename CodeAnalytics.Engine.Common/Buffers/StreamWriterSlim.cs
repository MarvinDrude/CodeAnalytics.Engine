using System.Buffers;
using System.Runtime.InteropServices;

namespace CodeAnalytics.Engine.Common.Buffers;

[StructLayout(LayoutKind.Auto)]
public ref struct StreamWriterSlim : IDisposable
{
   private readonly Stream _stream;
   private int _position;
   private readonly int _chunkSize;

   private readonly byte[] _buffer;
   private BufferWriter<byte> _writer;
   
   public StreamWriterSlim(
      Stream stream, int chunkSize = 2024 * 2024)
   {
      _stream = stream;
      _chunkSize = chunkSize;
      
      _position = 0;
      
      _buffer = ArrayPool<byte>.Shared.Rent(chunkSize);
      _writer = new BufferWriter<byte>(_buffer);
   }

   public void Write(scoped ReadOnlySpan<byte> span)
   {
      if (_position == 0)
      {
         _stream.Write(span);
         return;
      }
      
      if (NeedsFlushing(span.Length))
      {
         Flush();
      }
      
      _writer.Write(span);
      _position += span.Length;
   }

   public void Add(byte value)
   {
      if (_position == 0)
      {
         _stream.WriteByte(value);
         return;
      }
      
      if (NeedsFlushing(1))
      {
         Flush();
      }
      
      _writer.Add(value);
      _position += 1;
   }

   public Span<byte> AcquireSpan(int length)
   {
      if (NeedsFlushing(length))
      {
         Flush();
         
         if (NeedsFlushing(length)) 
            throw new InvalidOperationException("Required span size exceeds chunk size");
      }

      _position += length;
      return _writer.AcquireSpan(length, true);
   }

   public void Fill(byte value)
   {
      _writer.Fill(value);
   }
   
   public void Flush()
   {
      if (_position <= 0)
      {
         return;
      }

      _stream.Write(_writer.WrittenSpan);
      
      _position = 0;
      _writer.Position = 0;
      
      _stream.Flush();
   }
   
   public bool NeedsFlushing(int newAddition)
   {
      return newAddition + _position >= _chunkSize;
   }
   
   public void Dispose()
   {
      Flush();
      ArrayPool<byte>.Shared.Return(_buffer);
   }
}
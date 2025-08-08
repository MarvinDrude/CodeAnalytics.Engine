using System.IO.Compression;
using CodeAnalytics.Engine.Contracts.Compression;

namespace CodeAnalytics.Engine.Compression;

public sealed class DeflateCompressor : ICompressor
{
   public Memory<byte> Compress(Memory<byte> input)
   {
      using var memory = new MemoryStream();
      using (var compressed = new DeflateStream(memory, CompressionLevel.Optimal, leaveOpen: true))
      {
         compressed.Write(input.Span);
      }

      return memory.ToArray();
   }

   public Memory<byte> Decompress(Memory<byte> input)
   {
      using var memory = new MemoryStream(input.ToArray());
      using var decompressed = new DeflateStream(memory, CompressionMode.Decompress);
      using var result = new MemoryStream();
      
      decompressed.CopyTo(result);
      return result.ToArray();
   }
}
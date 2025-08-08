namespace CodeAnalytics.Engine.Contracts.Compression;

public interface ICompressor
{
   public Memory<byte> Compress(Memory<byte> input);
   
   public Memory<byte> Decompress(Memory<byte> input);
}
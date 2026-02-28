using System.Threading.Channels;
using Beskar.CodeAnalytics.Data.Bake.Sorting;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Extensions;
using Beskar.CodeAnalytics.Data.Serialization;
using Beskar.CodeAnalytics.Data.Syntax.Headers;
using Me.Memory.Buffers;
using Me.Memory.Extensions;

namespace Beskar.CodeAnalytics.Data.Discovery.Writers;

public sealed class SyntaxDiscoveryFileWriter : IAsyncDisposable
{
   public string FileName => FileNames.SyntaxFiles;

   private readonly Channel<SyntaxFile> _channel;
   private readonly Task _runningTask;

   private readonly FileStream _fileStream;
   private readonly string _filePath;
   private bool _disposed;
   
   public SyntaxDiscoveryFileWriter(string directoryPath)
   {
      _filePath = Path.Combine(directoryPath, FileName);
      
      _channel = Channel.CreateUnbounded<SyntaxFile>();
      _fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
      
      _runningTask = RunWriting();
   }

   public void Write(SyntaxFile syntaxFile)
   {
      _channel.Writer.TryWrite(syntaxFile);
   }
   
   public async Task Bake()
   {
      _channel.Writer.Complete();
      await _runningTask;

      await _fileStream.FlushAsync();
      _fileStream.Position = 0;

      var dictPath = _filePath + ".dict";
      var contentPath = _filePath + ".content";

      await using (var headerFile = new FileStream(dictPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
      await using (var contentFile = new FileStream(contentPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
      {
         var lengthBytes = new byte[sizeof(int)];
         
         while (_fileStream.Position < _fileStream.Length)
         {
            _fileStream.ReadExactly(lengthBytes.AsSpan());
            
            var length = lengthBytes.ReadLittleEndian<int>(out _);
            using var memoryOwner = new MemoryOwner<byte>(length);
            var span = memoryOwner.Span;
            
            _fileStream.ReadExactly(span);
            var fileId = span[..sizeof(uint)].ReadLittleEndian<uint>(out _);

            var header = new SyntaxDictionaryEntry()
            {
               FileId = fileId,
               Offset = (ulong)(_fileStream.Position - length),
               Length = length
            };
            headerFile.Write(header.AsBytes());

            contentFile.Write(lengthBytes.AsSpan());
            contentFile.Write(span);
         }
      }

      await _fileStream.DisposeAsync();
      File.Delete(_filePath);
      
      var dictSorted = $"{dictPath}.sorted";
      
      using var sorter = new FileSorter<SyntaxDictionaryEntry>(SyntaxDictionaryEntry.Comparer);
      sorter.Sort(dictPath, dictSorted);
      File.Delete(dictPath);

      await using (var final = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
      await using (var headerFile = new FileStream(dictSorted, FileMode.Open, FileAccess.Read, FileShare.Read))
      await using (var contentFile = new FileStream(contentPath, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
         using var memoryOwner = new MemoryOwner<byte>(sizeof(ulong));
         ((ulong)headerFile.Length + sizeof(ulong)).WriteLittleEndian(memoryOwner.Span);
         final.Write(memoryOwner.Span);

         await headerFile.CopyToAsync(final);
         await contentFile.CopyToAsync(final);
      }
      
      File.Delete(contentPath);
      _disposed = true;
   }
   
   private async Task RunWriting()
   {
      await foreach (var file in _channel.Reader.ReadAllAsync())
      {
         SyntaxFileSerializer.Serialize(file, (bytes) =>
         {
            _fileStream.Write(bytes);
         });
      }
   }

   public async ValueTask DisposeAsync()
   {
      if (_disposed) return;
      _disposed = true;
    
      // if something happened and bake was not run
      _channel.Writer.Complete();
      
      await _runningTask;
      await _fileStream.DisposeAsync();
   }
}
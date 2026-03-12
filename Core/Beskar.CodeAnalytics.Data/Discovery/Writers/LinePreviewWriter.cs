using System.Text;
using System.Threading.Channels;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Storage;
using Beskar.CodeAnalytics.Data.Metadata.Builders;
using Beskar.CodeAnalytics.Data.Metadata.Storage;

namespace Beskar.CodeAnalytics.Data.Discovery.Writers;

public sealed class LinePreviewWriter : IAsyncDisposable
{
   public string FileName => FileNames.LinePreviews;
   
   private readonly Channel<WriteRequest> _channel;
   private readonly Task _runningTask;

   private readonly FileStream _fileStream;
   private readonly BufferedStream _bufferedStream;
   
   private ulong _itemCount;
   private DatabaseBuilder _databaseBuilder;

   public LinePreviewWriter(DatabaseBuilder builder, string directoryPath)
   {
      var filePath = Path.Combine(directoryPath, FileName);

      _databaseBuilder = builder;
      
      _channel = Channel.CreateUnbounded<WriteRequest>();
      _fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
      _bufferedStream = new BufferedStream(_fileStream, 65536);
      
      _runningTask = RunWriting();
   }

   public ValueTask<LinePreviewView> Write(string line)
   {
      var cts = new TaskCompletionSource<LinePreviewView>(TaskCreationOptions.RunContinuationsAsynchronously);
      _channel.Writer.TryWrite(new WriteRequest(line, cts));
      
      return new ValueTask<LinePreviewView>(cts.Task);
   }

   private async Task RunWriting()
   {
      var buffer = new byte[4096];

      while (await _channel.Reader.WaitToReadAsync())
      {
         while (_channel.Reader.TryRead(out var request))
         {
            try
            {
               _itemCount++;
               
               var byteCount = Encoding.UTF8.GetByteCount(request.Line);
               if (byteCount > buffer.Length)
               {
                  buffer = new byte[Math.Max(buffer.Length * 2, byteCount)];
               }
            
               var span = buffer.AsSpan(0, byteCount);
               Encoding.UTF8.GetBytes(request.Line, span);
            
               var offset = _bufferedStream.Position;

               _bufferedStream.Write(span);
               request.CompletionSource.SetResult(new LinePreviewView((ulong)offset, span.Length));
            }
            catch (Exception e)
            {
               request.CompletionSource.SetException(e);
            }
         }
         
         await _bufferedStream.FlushAsync();
      }
   }
   
   public async ValueTask DisposeAsync()
   {
      _channel.Writer.Complete();
      
      await _runningTask;

      var length = _fileStream.Position;
      await _bufferedStream.DisposeAsync();
      await _fileStream.DisposeAsync();
      
      _databaseBuilder.Storage.Files.Add(new StorageFileDescriptor()
      {
         FileName = FileName,
         Kind = StorageFileKind.LinePreviews,
         LastModified = DateTimeOffset.UtcNow,
         ParentName = string.Empty,
         Name = "LinePreviews",
         ByteCount = (ulong)length,
         RowCount = _itemCount
      });
   }

   private readonly record struct WriteRequest(
      string Line, TaskCompletionSource<LinePreviewView> CompletionSource);
}
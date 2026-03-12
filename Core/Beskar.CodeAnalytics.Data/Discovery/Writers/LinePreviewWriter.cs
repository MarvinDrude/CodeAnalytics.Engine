using System.Text;
using System.Threading.Channels;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Misc;

namespace Beskar.CodeAnalytics.Data.Discovery.Writers;

public sealed class LinePreviewWriter : IAsyncDisposable
{
   public string FileName => FileNames.LinePreviews;
   
   private readonly Channel<WriteRequest> _channel;
   private readonly Task _runningTask;

   private readonly FileStream _fileStream;
   private readonly BufferedStream _bufferedStream;

   public LinePreviewWriter(string directoryPath)
   {
      var filePath = Path.Combine(directoryPath, FileName);
      
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
               var byteCount = Encoding.UTF8.GetByteCount(request.Line);
               if (byteCount > buffer.Length)
               {
                  buffer = new byte[Math.Max(buffer.Length * 2, byteCount)];
               }
            
               var span = buffer.AsSpan(0, byteCount);
               Encoding.UTF8.GetBytes(request.Line, span);
            
               var offset = _fileStream.Position + _bufferedStream.Position;

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
      await _fileStream.DisposeAsync();
   }

   private readonly record struct WriteRequest(
      string Line, TaskCompletionSource<LinePreviewView> CompletionSource);
}
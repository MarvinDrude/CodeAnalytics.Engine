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

   public LinePreviewWriter(string directoryPath)
   {
      var filePath = Path.Combine(directoryPath, FileName);
      
      _channel = Channel.CreateUnbounded<WriteRequest>();
      _fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
      
      _runningTask = RunWriting();
   }

   public async Task<LinePreviewView> Write(string line)
   {
      var cts = new TaskCompletionSource<LinePreviewView>(TaskCreationOptions.RunContinuationsAsynchronously);
      await _channel.Writer.WriteAsync(new WriteRequest(line, cts));
      
      return await cts.Task;
   }

   private async Task RunWriting()
   {
      await foreach (var request in _channel.Reader.ReadAllAsync())
      {
         try
         {
            var bytes = Encoding.UTF8.GetBytes(request.Line);
            var offset = _fileStream.Position;

            await _fileStream.WriteAsync(bytes);
            var length = bytes.Length;

            request.CompletionSource.SetResult(new LinePreviewView((ulong)offset, length));
         }
         catch (Exception e)
         {
            request.CompletionSource.SetException(e);
         }
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
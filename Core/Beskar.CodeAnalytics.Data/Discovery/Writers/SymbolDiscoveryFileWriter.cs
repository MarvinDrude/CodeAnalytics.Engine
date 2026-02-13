using System.Threading.Channels;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Extensions;

namespace Beskar.CodeAnalytics.Data.Discovery.Writers;

public sealed class SymbolDiscoveryFileWriter<TKey, TSymbol> : IAsyncDisposable
   where TSymbol : unmanaged
   where TKey : IEquatable<TKey>
{
   private static readonly string _fileName = $"{typeof(TSymbol).Name.ToLowerInvariant()}.discovery.mmb";
   
   private readonly Channel<(TKey Id, TSymbol Symbol)> _channel;
   private readonly Task _runningTask;
   private readonly DiscoveryFileWriter<TKey> _fileWriter;

   public SymbolDiscoveryFileWriter(string directoryPath)
   {
      _channel = Channel.CreateUnbounded<(TKey id, TSymbol symbol)>(new UnboundedChannelOptions()
      {
         SingleReader = true
      });
      
      _fileWriter = new DiscoveryFileWriter<TKey>(Path.Combine(directoryPath, _fileName));
      _runningTask = RunWriting();
   }

   public ValueTask Write(TKey id, TSymbol symbol)
   {
      return _channel.Writer.WriteAsync((id, symbol));
   }
   
   private async Task RunWriting()
   {
      await foreach (var (id, symbol) in _channel.Reader.ReadAllAsync())
      {
         // no need to await since we full sync here
         var task = _fileWriter.Write(id, (fs) =>
         {
            var copy = symbol;
            fs.Write(copy.AsBytes());
            
            return ValueTask.CompletedTask;  
         });

         if (!task.IsCompletedSuccessfully)
         {
            throw new InvalidOperationException();
         }
      }
   }

   public async ValueTask DisposeAsync()
   {
      _channel.Writer.Complete();
      await _runningTask;
      _fileWriter.Dispose();
   }
}
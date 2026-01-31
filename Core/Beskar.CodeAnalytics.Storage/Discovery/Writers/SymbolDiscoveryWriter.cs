using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Beskar.CodeAnalytics.Storage.Extensions;

namespace Beskar.CodeAnalytics.Storage.Discovery.Writers;

public sealed class SymbolDiscoveryWriter<TSymbol> : IDisposable
   where TSymbol : unmanaged
{
   private readonly SemaphoreSlim _semaphore = new(1, 1);
   
   private static readonly string _fileName = $"{typeof(TSymbol).Name.ToLowerInvariant()}.discovery.mmb";
   private readonly DiscoveryFileWriter _fileWriter;

   public SymbolDiscoveryWriter(string folderPath)
   {
      var filePath = Path.Combine(folderPath, _fileName);
      File.Delete(filePath);
      
      _fileWriter = new DiscoveryFileWriter(filePath);
   }
   
   public async Task Write(ulong id, TSymbol symbol)
   {
      await _semaphore.WaitAsync();

      try
      {
         await _fileWriter.Write(id, (fs) =>
         {
            var bytes = symbol.AsBytes();
            fs.Write(bytes);

            return Task.CompletedTask;
         });
      }
      finally
      {
         _semaphore.Release();
      }
   }
   
   public void Dispose()
   {
      _semaphore.Dispose();
      _fileWriter.Dispose();
   }
}
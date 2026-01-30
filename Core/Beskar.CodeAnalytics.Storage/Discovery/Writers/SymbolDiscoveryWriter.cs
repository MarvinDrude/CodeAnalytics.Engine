using Beskar.CodeAnalytics.Storage.Extensions;

namespace Beskar.CodeAnalytics.Storage.Discovery.Writers;

public sealed class SymbolDiscoveryWriter<TSymbol> : IDisposable
   where TSymbol : unmanaged
{
   private static readonly string _fileName = $"{typeof(TSymbol).Name.ToLowerInvariant()}.discovery.mmb";
   private readonly DiscoveryFileWriter _fileWriter;

   public SymbolDiscoveryWriter(string folderPath)
   {
      var filePath = Path.Combine(folderPath, _fileName);
      _fileWriter = new DiscoveryFileWriter(filePath);
      
      File.Delete(filePath);
   }
   
   public async Task Write(ulong id, TSymbol symbol)
   {
      await _fileWriter.Write(id, (fs) =>
      {
         var bytes = symbol.AsBytes();
         fs.Write(bytes);
         
         return Task.CompletedTask;
      });
   }
   
   public void Dispose()
   {
      _fileWriter.Dispose();
   }
}
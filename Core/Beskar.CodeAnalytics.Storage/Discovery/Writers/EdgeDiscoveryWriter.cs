using Beskar.CodeAnalytics.Storage.Entities.Edges;
using Beskar.CodeAnalytics.Storage.Extensions;

namespace Beskar.CodeAnalytics.Storage.Discovery.Writers;

public sealed class EdgeDiscoveryWriter : IDisposable
{
   public string FilePath { get; }
   
   private const string _fileName = "edges.discovery.mmb";
   private readonly Lock _lock = new();

   private readonly FileStream _fileStream;
   private readonly HashSet<EdgeKey> _already = [];
   
   public EdgeDiscoveryWriter(string folderPath)
   {
      var filePath = Path.Combine(folderPath, _fileName);
      File.Delete(filePath);
      
      _fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Write);
      FilePath = filePath;
   }
   
   public bool Write(ref EdgeDefinition edge)
   {
      lock (_lock)
      {
         if (!_already.Add(edge.Key))
         {
            return false;
         }
         
         _fileStream.Write(edge.AsBytes());
         return true;
      }
   }
   
   public void Dispose()
   {
      _already.Clear();
      _fileStream.Dispose();
   }
}
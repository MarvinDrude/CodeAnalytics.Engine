using System.Text;

namespace Beskar.CodeAnalytics.Storage.Discovery.Writers;

public sealed class DiscoveryFileWriter(string filePath) : IDisposable
{
   private readonly FileStream _file = new (filePath, FileMode.Append, FileAccess.Write, FileShare.Write);
   
   private readonly Dictionary<ulong, long> _offsets = [];

   public ValueTask<bool> Write(ulong id, Func<FileStream, Task> writeAction)
   {
      return !_offsets.TryAdd(id, _file.Position) 
         ? new ValueTask<bool>(false) 
         : Awaited();

      async ValueTask<bool> Awaited()
      {
         await writeAction(_file);
         return true;
      }
   }
   
   public void Dispose()
   {
      _file.Dispose();
   }
}
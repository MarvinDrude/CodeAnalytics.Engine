namespace Beskar.CodeAnalytics.Data.Discovery.Writers;

public sealed class DiscoveryFileWriter(string filePath) : IDisposable
{
   private readonly FileStream _file = new (filePath, FileMode.Append, FileAccess.Write, FileShare.Write);
   private readonly Dictionary<uint, long> _offsets = [];

   public ValueTask<bool> Write(uint id, Func<FileStream, ValueTask> writeAction)
   {
      return !_offsets.TryAdd(id, _file.Position) 
         ? new ValueTask<bool>(false) 
         : Run();

      ValueTask<bool> Run()
      {
         var originTask = writeAction(_file);
         return originTask.IsCompletedSuccessfully 
            ? ValueTask.FromResult(true) : Awaited(originTask);
      }

      async ValueTask<bool> Awaited(ValueTask originTask)
      {
         await originTask;
         return true;
      }
   }
   
   public void Dispose()
   {
      _file.Dispose();
   }
}
using System.IO.MemoryMappedFiles;

namespace Beskar.CodeAnalytics.Data.Files;

public sealed unsafe class MmfHandle : IDisposable
{
   private readonly FileStream _fileStream;
   private readonly MemoryMappedFile _mmf;
   private readonly MemoryMappedViewAccessor _accessor;

   public long Length => _fileStream.Length;
   
   public MmfHandle(string filePath, bool writable = false)
   {
      var access = writable ? FileAccess.ReadWrite : FileAccess.Read;
      var mapAccess = writable ? MemoryMappedFileAccess.ReadWrite : MemoryMappedFileAccess.Read;

      _fileStream = new FileStream(filePath, FileMode.Open, access, FileShare.ReadWrite);
      _mmf = MemoryMappedFile.CreateFromFile(
         _fileStream, 
         mapName: null, 
         capacity: 0, 
         mapAccess, 
         HandleInheritability.None, 
         leaveOpen: false);
      
      _accessor = _mmf.CreateViewAccessor(0, 0, mapAccess);
   }
   
   public MmfBuffer GetBuffer() => new(_accessor);

   public void ProcessInBatches<T>(int batchSize, Action<Span<T>> processAction)
      where T : unmanaged
   {
      using var buffer = GetBuffer();
      
      var totalBytes = Length;
      var structSize = sizeof(T);
      var totalStructs = (int)(totalBytes / structSize);
      
      for (var i = 0; i < totalStructs; i += batchSize)
      {
         var remaining = totalStructs - i;
         var currentBatchCount = Math.Min(batchSize, remaining);
         
         var offset = (long)i * structSize;
         var batch = buffer.GetSpan<T>(offset, currentBatchCount);
         
         processAction(batch);
      }
   }
   
   public void Dispose()
   {
      _accessor.Flush();
      _accessor.Dispose();
      
      _mmf.Dispose();
      _fileStream.Dispose();
   }
}
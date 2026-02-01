using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Extensions;

namespace Beskar.CodeAnalytics.Storage.Discovery.Sorting;

public sealed unsafe class FileSorter<T> : IDisposable
   where T : unmanaged
{
   private static readonly int _structSize = sizeof(T);
   private readonly IComparer<T> _comparison;
   
   private readonly List<string> _tempFiles = [];

   public FileSorter(IComparer<T> comparison)
   {
      _comparison = comparison;
   }

   public void Sort(string sourceFilePath, string targetFilePath, int itemBuffserSize)
   {
      SplitAndSort(sourceFilePath, itemBuffserSize);
      MergeFiles(_tempFiles, targetFilePath);
   }

   private void SplitAndSort(string sourceFilePath, int itemBuffserSize)
   {
      using var fs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read);
      var buffer = new byte[itemBuffserSize * _structSize];
      
      fixed (byte* pBuffer = buffer)
      {
         var items = (T*)pBuffer;
         int bytesRead;
         
         while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
         {
            var count = bytesRead / _structSize;
            
            var span = new Span<T>(items, count);
            span.Sort(_comparison);

            var tempPath = Path.GetTempFileName();
            using var outFs = new FileStream(tempPath, FileMode.Create);
            
            outFs.Write(buffer, 0, bytesRead);
            _tempFiles.Add(tempPath);
         }
      }
   }

   private void MergeFiles(List<string> tempFiles, string targetFilePath)
   {
      var pq = new PriorityQueue<(T Item, int FileIndex), T>(_comparison);

      var readers = tempFiles.Select(File.OpenRead).ToArray();
      using var writer = File.OpenWrite(targetFilePath);

      try
      {
         for (var i = 0; i < readers.Length; i++)
         {
            if (TryReadNext(readers[i], out var item))
               pq.Enqueue((item, i), item);
         }

         while (pq.Count > 0)
         {
            var (item, index) = pq.Dequeue();
            writer.Write(MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref item, 1)));

            if (TryReadNext(readers[index], out var nextItem))
               pq.Enqueue((nextItem, index), nextItem);
         }
      }
      finally
      {
         foreach (var r in readers) 
            r.Dispose();
      }
   }
   
   private bool TryReadNext(FileStream fs, out T item)
   {
      item = default;

      var buffer = new byte[_structSize];
      var read = fs.Read(buffer);
      
      if (read < _structSize) return false;
      
      item = buffer.AsSpan().AsStruct<T>();
      return true;
   }

   public void Dispose()
   {
      foreach (var f in _tempFiles) File.Delete(f);
   }
}
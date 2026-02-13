using System.Diagnostics.CodeAnalysis;
using System.IO.MemoryMappedFiles;

namespace Beskar.CodeAnalytics.Data.Hashing;

public sealed class StringFileWriter : IDisposable
{
   private readonly Lock _lock = new();
   private readonly Dictionary<ulong, List<long>> _registry = [];
   
   private readonly string _filePath;
   private long _capacity;
   private long _length;
   
   private MemoryMappedFile _file;
   private MemoryMappedViewAccessor _accessor;
   
   public StringFileWriter(
      string filePath, long initialCapacity = 1024 * 1024 * 10)
   {
      _filePath = filePath;
      File.Delete(_filePath);
      
      _capacity = initialCapacity;
      
      ReMapFile();
   }
   
   [MemberNotNull(nameof(_file), nameof(_accessor))]
   private void ReMapFile()
   {
      _accessor?.Flush();
      _accessor?.Dispose();
      
      _file?.Dispose();
      
      _file = MemoryMappedFile.CreateFromFile(_filePath, FileMode.OpenOrCreate, null, _capacity,
         MemoryMappedFileAccess.ReadWrite);
      _accessor = _file.CreateViewAccessor(0, _capacity, MemoryMappedFileAccess.ReadWrite);
   }
}
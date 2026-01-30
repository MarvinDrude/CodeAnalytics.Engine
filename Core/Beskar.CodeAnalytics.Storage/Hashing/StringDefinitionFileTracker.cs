using System.Diagnostics.CodeAnalysis;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Text;
using Beskar.CodeAnalytics.Storage.Entities.Misc;
using Beskar.CodeAnalytics.Storage.Extensions;
using Me.Memory.Buffers;
using Me.Memory.Extensions;

namespace Beskar.CodeAnalytics.Storage.Hashing;

public sealed class StringDefinitionFileTracker : IDisposable
{
   private readonly Lock _lock = new();
   private readonly Dictionary<ulong, List<long>> _registry = [];

   private readonly string _filePath;
   private long _capacity;
   private long _length;
   
   private MemoryMappedFile _file;
   private MemoryMappedViewAccessor _accessor;
   
   public StringDefinitionFileTracker(
      string filePath, long initialCapacity = 1024 * 1024 * 10)
   {
      _filePath = filePath;
      _capacity = initialCapacity;
      
      ReMapFile();
   }

   public StringDefinition GetStringDefinition(scoped in ReadOnlySpan<char> str)
   {
      lock (_lock)
      {
         var hash = DeterministicHasher.GetDeterministicId(str, 7331L);
      
         var requiredSize = Encoding.UTF8.GetByteCount(str);
         using var owner = requiredSize <= 1024
            ? new SpanOwner<byte>(stackalloc byte[requiredSize])
            : new SpanOwner<byte>(requiredSize);

         Encoding.UTF8.GetBytes(str, owner.Span);

         if (_registry.TryGetValue(hash, out var offsets))
         {
            foreach (var offset in offsets)
            {
               if (!IsMatch(owner.Span, offset)) continue;
               return new StringDefinition((ulong)offset);
            }
         }

         return new StringDefinition((ulong)Append(owner.Span, hash));
      }
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
   
   private bool IsMatch(ReadOnlySpan<byte> bytes, long offset)
   {
      var span = _accessor.AcquireSpan<byte>(offset + sizeof(int), bytes.Length);
      return span.SequenceEqual(bytes);
   }
   
   private long Append(scoped in ReadOnlySpan<byte> bytes, ulong hash)
   {
      if (_length + bytes.Length > _capacity)
      {
         _capacity *= 2;
         ReMapFile();
      }

      var offset = _length;
      
      Span<byte> lengthBytes = stackalloc byte[sizeof(int)];
      bytes.Length.WriteLittleEndian(lengthBytes);

      var addLength = lengthBytes.Length + bytes.Length;
      var destination = _accessor.AcquireSpan<byte>(offset, addLength);
      
      lengthBytes.CopyTo(destination);
      bytes.CopyTo(destination[lengthBytes.Length..]);
      
      _length += addLength;
      if (!_registry.TryGetValue(hash, out var list))
      {
         list = _registry[hash] = [];
      }

      list.Add(offset);
      return offset;
   }

   public void Dispose()
   {
      _accessor.Flush();
      _accessor.Dispose();
      
      _file.Dispose();
      
      // Shrink file to actual used size on disk
      using var fs = new FileStream(_filePath, FileMode.Open);
      fs.SetLength(_length);
   }
}
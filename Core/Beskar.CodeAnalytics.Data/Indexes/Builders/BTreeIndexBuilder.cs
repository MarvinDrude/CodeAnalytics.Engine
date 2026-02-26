using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Bake.Sorting;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Extensions;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Indexes.BTree;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Me.Memory.Buffers;
using Me.Memory.Extensions;

namespace Beskar.CodeAnalytics.Data.Indexes.Builders;

public sealed class BTreeIndexBuilder<TEntity, TKey>
   where TEntity : unmanaged, ISpec
   where TKey : unmanaged, IComparable<TKey>
{
   private const int PageSize = 4096;
   
   private readonly BakeContext _context;
   private readonly Func<TEntity, TKey> _keySelector;
   private readonly IComparer<KeyedIndexEntry<TKey>> _comparer;
   
   private readonly string _sourceFilePath;
   private readonly string _unorderedFilePath;
   private readonly string _orderedFilePath;
   private readonly string _finalFilePath;

   public BTreeIndexBuilder(
      BakeContext context, FileId sourceFileId,
      Func<TEntity, TKey> keySelector, string indexName,
      IComparer<KeyedIndexEntry<TKey>> comparer)
   {
      _context = context;
      _keySelector = keySelector;
      _comparer = comparer;
      
      _sourceFilePath = Path.Combine(_context.OutputDirectoryPath, _context.FileNames[sourceFileId]);
      var sourceFolder = Path.GetDirectoryName(_sourceFilePath) ?? "";
      var sourceName = Path.GetFileName(_sourceFilePath);
      
      _unorderedFilePath = $"{_sourceFilePath}.{indexName}.tempunordered";
      _orderedFilePath = $"{_sourceFilePath}.{indexName}.tempordered";
      
      _finalFilePath = Path.Combine(sourceFolder, $"index_{sourceName.GetBaseFileName()}.{indexName}.mmb");
   }

   public void Build()
   {
      using (var sourceHandle = new MmfHandle(_sourceFilePath, writable: false))
      using (var tempUnorderedFile = new FileStream(_unorderedFilePath, FileMode.Create, FileAccess.Write))
      {
         WriteUnorderedTempFile(sourceHandle, tempUnorderedFile);
      }
      
      SortTempFileExternally();
      
      using (var orderedSource = new FileStream(_orderedFilePath, FileMode.Open, FileAccess.Read))
      using (var finalFile = new FileStream(_finalFilePath, FileMode.Create, FileAccess.ReadWrite))
      {
         finalFile.SetLength(PageSize); // reserve space
         finalFile.Position = PageSize;

         var leaveOffsets = WriteLeaves(orderedSource, finalFile);
         var rootOffset = WriteBranches(leaveOffsets, finalFile);

         finalFile.Position = 0;
         
         Span<byte> buffer = stackalloc byte[sizeof(long)];
         rootOffset.WriteLittleEndian(buffer);
         
         finalFile.Write(buffer);
      }
      
      File.Delete(_orderedFilePath);
   }

   private long WriteBranches(List<PageBoundary> offsets, FileStream final)
   {
      if (offsets.Count <= 1) return offsets[0].Offset;

      List<PageBoundary> parentOffsets = [];
      using var pageOwner = new MemoryOwner<byte>(PageSize);
      var pageSpan = pageOwner.Span;
      
      var branchEntrySize = Unsafe.SizeOf<BTreeEntry<TKey>>();
      var pageHeaderSize = Unsafe.SizeOf<BTreePageHeader>();
      var maxEntries = (PageSize - Unsafe.SizeOf<BTreePageHeader>()) / branchEntrySize;

      for (var e = 0; e < offsets.Count;)
      {
         var currentOffset = final.Position;
         pageSpan.Clear();

         ref var header = ref Unsafe.As<byte, BTreePageHeader>(ref pageSpan[0]);
         header.Type = BTreePageType.Branch;

         var dataSpan = pageSpan[pageHeaderSize..];
         var count = 0;

         while (count < maxEntries && e < offsets.Count)
         {
            var (maxKey, offset) = offsets[e];

            var entry = new BTreeEntry<TKey>()
            {
               Key = maxKey,
               PageOffset = offset
            };
            entry.AsBytes().CopyTo(dataSpan[(count * branchEntrySize)..]);

            count++;
            e++;
         }
         
         ref var lastEntry = ref Unsafe.As<byte, BTreeEntry<TKey>>(ref dataSpan[(count - 1) * branchEntrySize]);
         parentOffsets.Add(new PageBoundary(lastEntry.Key, currentOffset));
         
         header.ItemCount = count;
         final.Write(pageSpan);
      }
      
      return WriteBranches(parentOffsets, final);
   }

   private List<PageBoundary> WriteLeaves(FileStream sortedFile, FileStream final)
   {
      List<PageBoundary> offsets = [];
      using var pageOwner = new MemoryOwner<byte>(PageSize);
      var pageSpan = pageOwner.Span;
      
      var entrySize = Unsafe.SizeOf<KeyedIndexEntry<TKey>>();
      var pageHeaderSize = Unsafe.SizeOf<BTreePageHeader>();
      var maxEntries = (PageSize - Unsafe.SizeOf<BTreePageHeader>()) / entrySize;

      while (sortedFile.Position < sortedFile.Length)
      {
         var currentOffset = final.Position;
         pageSpan.Clear();

         ref var header = ref Unsafe.As<byte, BTreePageHeader>(ref pageSpan[0]);
         header.Type = BTreePageType.Leaf;

         var count = 0;
         var dataSpan = pageSpan[pageHeaderSize..];

         while (count < maxEntries 
                && sortedFile.Position < sortedFile.Length)
         {
            sortedFile.ReadExactly(dataSpan.Slice(count * entrySize, entrySize));
            count++;
         }
         
         ref var lastEntry = ref Unsafe.As<byte, KeyedIndexEntry<TKey>>(ref dataSpan[(count - 1) * entrySize]);
         offsets.Add(new PageBoundary(lastEntry.Key, currentOffset));

         header.ItemCount = count;
         header.NextPageOffset = (sortedFile.Position < sortedFile.Length) 
            ? currentOffset + PageSize 
            : 0;
         
         final.Write(pageSpan);
      }

      return offsets;
   }

   private void SortTempFileExternally()
   {
      using var sorter = new FileSorter<KeyedIndexEntry<TKey>>(_comparer);
      sorter.Sort(_unorderedFilePath, _orderedFilePath);

      File.Delete(_unorderedFilePath);
   }
   
   private void WriteUnorderedTempFile(MmfHandle sourceHandle, FileStream tempUnorderedFile)
   {
      sourceHandle.ProcessInBatches(8 * 512, (Span<TEntity> span) =>
      {
         foreach (ref var entity in span)
         {
            var keyValue = _keySelector(entity);
            var entry = new KeyedIndexEntry<TKey>()
            {
               Id = entity.Identifier,
               Key = keyValue,
            };
            
            var byteSpan = entry.AsBytes();
            tempUnorderedFile.Write(byteSpan);
         }
      });
   }

   private record struct PageBoundary(TKey MaxKey, long Offset);
}
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Bake.Sorting;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Enums.Indexes;
using Beskar.CodeAnalytics.Data.Extensions;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Indexes.Models;

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
      var levelFilePaths = BuildLevels();
      File.Delete(_orderedFilePath);
      
      MergeFinalFile(levelFilePaths);
      foreach (var levelFilePath in levelFilePaths)
      {
         File.Delete(levelFilePath);
         File.Delete(levelFilePath + ".parents");
      }
   }

   private void MergeFinalFile(List<string> levelFilePaths)
   {
      using var finalFile = new FileStream(_finalFilePath, FileMode.Create, FileAccess.Write);
      
      var internalNodesTotalSize = 0L;
      for (var i = 1; i < levelFilePaths.Count; i++)
      {
         internalNodesTotalSize += new FileInfo(levelFilePaths[i]).Length;
      }
      
      var headerSize = Unsafe.SizeOf<IndexHeader>();
      var header = new IndexHeader
      {
         Type = IndexType.StaticWideBTree,
         DictionaryOffset = (ulong)headerSize,
         DataOffset = (ulong)(headerSize + internalNodesTotalSize)
      };
      
      finalFile.Write(header.AsBytes());
      // Root -> internal -> leaves
      levelFilePaths.Reverse();

      foreach (var levelFilePath in levelFilePaths)
      {
         using var src = new FileStream(levelFilePath, FileMode.Open, FileAccess.Read);
         src.CopyTo(finalFile);
      }
   }

   private List<string> BuildLevels()
   {
      List<string> levels = [];
      var currentInput = _orderedFilePath;
      var levelIndex = 0;

      while (true)
      {
         var levelPath = $"{_orderedFilePath}.level{levelIndex}";
         var parentInputPath = $"{_orderedFilePath}.level{levelIndex}.parents";
         
         var pageCount = PackLevel(currentInput, levelPath, parentInputPath);
         levels.Add(levelPath);
         
         if (pageCount <= 1) break;
         
         currentInput = parentInputPath;
         levelIndex++;
      }

      return levels;
   }

   private long PackLevel(string inputPath, string outputPath, string parentEntriesPath)
   {
      using var reader = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
      using var writer = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
      using var parentWriter = new FileStream(parentEntriesPath, FileMode.Create, FileAccess.Write);
      
      var entrySize = Unsafe.SizeOf<KeyedIndexEntry<TKey>>();
      var maxEntriesPerPage = (PageSize - sizeof(int)) / entrySize;
      
      var pageBuffer = new byte[PageSize];
      var entryBuffer = new byte[entrySize];
      var pageCount = 0L;
      
      while (reader.Position < reader.Length)
      {
         var entriesInPage = 0;
         TKey firstKeyInPage = default;

         while (entriesInPage < maxEntriesPerPage && reader.Read(entryBuffer, 0, entrySize) > 0)
         {
            if (entriesInPage == 0)
            {
               firstKeyInPage = Unsafe.As<byte, KeyedIndexEntry<TKey>>(ref entryBuffer[0]).Key;
            }

            Array.Copy(entryBuffer, 0, pageBuffer, sizeof(int) + (entriesInPage * entrySize), entrySize);
            entriesInPage++;
         }

         BinaryPrimitives.WriteInt32LittleEndian(pageBuffer.AsSpan(0, 4), entriesInPage);
         writer.Write(pageBuffer, 0, PageSize);

         var parentEntry = new KeyedIndexEntry<TKey> { Key = firstKeyInPage, Id = (uint)pageCount };
         parentWriter.Write(parentEntry.AsBytes());

         pageCount++;
      }

      return pageCount;
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
}
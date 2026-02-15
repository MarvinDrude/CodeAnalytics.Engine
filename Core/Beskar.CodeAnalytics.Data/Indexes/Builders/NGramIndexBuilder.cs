using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Bake.Sorting;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Extensions;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Indexes.Intermediate;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Data.Indexes.Builders;

public sealed class NGramIndexBuilder<TEntity>
   where TEntity : unmanaged, ISpec
{
   private readonly BakeContext _context;
   private readonly Func<TEntity, StringFileView> _selector;

   private readonly string _indexName;
   
   private readonly string _sourceFullFilePath;
   private readonly string _unorderedFilePath;
   private readonly string _orderedFilePath;
   
   public NGramIndexBuilder(
      BakeContext context, FileId sourceFileId,
      Func<TEntity, StringFileView> selector,
      string indexName)
   {
      _context = context;
      _selector = selector;
      _indexName = indexName;
      
      _sourceFullFilePath = Path.Combine(_context.OutputDirectoryPath, _context.FileNames[sourceFileId]);
      _unorderedFilePath = $"{_sourceFullFilePath}.{_indexName}.tempunordered";
      _orderedFilePath = $"{_sourceFullFilePath}.{_indexName}.tempordered";
   }

   public void Build()
   {
      using var sourceHandle = new MmfHandle(_sourceFullFilePath, writable: false);
      using (var tempUnorderedFile = new FileStream(_unorderedFilePath, FileMode.Create, FileAccess.Write))
      {
         WriteUnorderedTempFile(sourceHandle, tempUnorderedFile);
      }
      
      SortTempFileExternally();
      WriteTempPairFiles();
   }

   private void WriteTempPairFiles()
   {
      
   }
   
   private void SortTempFileExternally()
   {
      using var sorter = new FileSorter<KeyedIndexEntry<NGram3>>(_comparer);
      sorter.Sort(_unorderedFilePath, _orderedFilePath);
      
      File.Delete(_unorderedFilePath);
   }

   private void WriteUnorderedTempFile(MmfHandle sourceHandle, FileStream tempUnorderedFile)
   {
      sourceHandle.ProcessInBatches(8 * 512, (Span<TEntity> span) =>
      {
         foreach (ref var entity in span)
         {
            var stringView = _selector(entity);
            var str = _context.GetString(stringView);
            var ngrams = NGramHelper.CreateNGrams<NGram3>(str.AsSpan(), entity.Identifier, 3);
            var byteSpan = ngrams.AsSpan().AsBytes();
            
            tempUnorderedFile.Write(byteSpan);
         }
      });
   }

   private static readonly IComparer<KeyedIndexEntry<NGram3>> _comparer = Comparer<KeyedIndexEntry<NGram3>>.Create(
      static (x, y) => x.Key.MaterializedString.CompareTo(y.Key.MaterializedString, StringComparison.OrdinalIgnoreCase));
}
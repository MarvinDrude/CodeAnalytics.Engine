using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Bake.Sorting;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Indexes;
using Beskar.CodeAnalytics.Data.Extensions;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Indexes.Intermediate;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Me.Memory.Extensions;

namespace Beskar.CodeAnalytics.Data.Indexes.Builders;

public sealed class NGramIndexBuilder<TEntity>
   where TEntity : unmanaged, ISpec
{
   private readonly BakeContext _context;
   private readonly Func<TEntity, StringFileView> _selector;

   private readonly string _sourceFullFilePath;
   private readonly string _unorderedFilePath;
   private readonly string _orderedFilePath;

   private readonly string _postingsFilePath;
   private readonly string _dictionaryFilePath;

   private readonly string _finalFilePath;
   
   public NGramIndexBuilder(
      BakeContext context, FileId sourceFileId,
      Func<TEntity, StringFileView> selector,
      string indexName)
   {
      _context = context;
      _selector = selector;
      
      _sourceFullFilePath = Path.Combine(_context.OutputDirectoryPath, _context.FileNames[sourceFileId]);
      var sourceFolder = Path.GetDirectoryName(_sourceFullFilePath) ?? "";
      var sourceName = Path.GetFileName(_sourceFullFilePath);
      
      _unorderedFilePath = $"{_sourceFullFilePath}.{indexName}.tempunordered";
      _orderedFilePath = $"{_sourceFullFilePath}.{indexName}.tempordered";
      
      _postingsFilePath = $"{_sourceFullFilePath}.{indexName}.postings";
      _dictionaryFilePath = $"{_sourceFullFilePath}.{indexName}.dictionary";
      
      _finalFilePath = Path.Combine(sourceFolder, $"index_{sourceName.GetBaseFileName()}.{indexName}.mmb");
   }

   public void Build()
   {
      using (var sourceHandle = new MmfHandle(_sourceFullFilePath, writable: false))
      using (var tempUnorderedFile = new FileStream(_unorderedFilePath, FileMode.Create, FileAccess.Write))
      {
         WriteUnorderedTempFile(sourceHandle, tempUnorderedFile);
      }
      
      SortTempFileExternally();

      using (var tempOrderedHandle = new MmfHandle(_orderedFilePath, writable: false))
      using (var postingsStream = new FileStream(_postingsFilePath, FileMode.Create, FileAccess.Write))
      using (var dictStream = new FileStream(_dictionaryFilePath, FileMode.Create, FileAccess.Write))
      {
         WriteTempPairFiles(tempOrderedHandle, postingsStream, dictStream);
      }

      MergeFinalFile();
      
      File.Delete(_unorderedFilePath);
      File.Delete(_orderedFilePath);
      File.Delete(_postingsFilePath);
      File.Delete(_dictionaryFilePath);
   }

   private void MergeFinalFile()
   {
      using var finalFile = new FileStream(_finalFilePath, FileMode.Create, FileAccess.Write);
      using var dictSource = new FileStream(_dictionaryFilePath, FileMode.Open, FileAccess.Read);
      using var postingsSource = new FileStream(_postingsFilePath, FileMode.Open, FileAccess.Read);

      var dictSize = dictSource.Length;
      
      var headerSize = Unsafe.SizeOf<IndexHeader>();
      var header = new IndexHeader()
      {
         Type = IndexType.NGram,
         DictionaryOffset = (ulong)headerSize,
         KeySize = (uint)Unsafe.SizeOf<NGram3>(),
         DataOffset = (ulong)(headerSize + dictSize),
         EntrySize = sizeof(uint)
      };

      finalFile.Write(header.AsBytes());
      
      dictSource.CopyTo(finalFile);
      postingsSource.CopyTo(finalFile);
   }

   private void WriteTempPairFiles(MmfHandle tempOrderedHandle, 
      FileStream postingsStream, FileStream dictionaryStream)
   {
      var isFirst = true;
      HashSet<uint> already = [];
      var dictionary = new DictionaryEntry<NGram3>()
      {
         Key = default,
         Count = 0,
         Offset = 0
      };
      
      tempOrderedHandle.ProcessInBatches(8 * 512, (Span<KeyedIndexEntry<NGram3>> span) =>
      {
         Span<byte> idSpan = stackalloc byte[sizeof(uint)];
         
         foreach (ref var entity in span)
         {
            if (isFirst)
            {
               dictionary.Key = entity.Key;
               isFirst = false;
               continue;
            }

            if (!NGramEquality.EqualsFast(ref dictionary.Key, ref entity.Key))
            {
               dictionaryStream.Write(dictionary.AsBytes());

               dictionary.Offset += sizeof(uint) * dictionary.Count;
               dictionary.Count = 0;
               dictionary.Key = entity.Key;

               already.Clear();
            }
            else if (!already.Add(entity.Id)) // no dups
            {
               continue;
            }
            
            entity.Id.WriteLittleEndian(idSpan);
            postingsStream.Write(idSpan);
               
            dictionary.Count++;
         }
      });

      if (!isFirst)
      {
         dictionaryStream.Write(dictionary.AsBytes());
      }
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
      static (x, y) =>
      {
         var first = NGramEquality.CompareFast(ref x.Key, ref y.Key, 12);
         if (first != 0) return first;
         
         return x.Id.CompareTo(y.Id);
      });
}
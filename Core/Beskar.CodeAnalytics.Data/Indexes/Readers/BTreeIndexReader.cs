using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Beskar.CodeAnalytics.Data.Indexes.Search;

namespace Beskar.CodeAnalytics.Data.Indexes.Readers;

public sealed class BTreeIndexReader<TKey> : IDisposable
   where TKey : unmanaged, IComparable<TKey>
{
   private const int PageSize = 4096;
   
   private readonly MmfHandle _handle;
   private readonly IndexHeader _header;
   
   private readonly int _entrySize;

   public BTreeIndexReader(string filePath)
   {
      _handle = new MmfHandle(filePath, writable: false);

      using var buffer = _handle.GetBuffer();
      _header = buffer.GetRef<IndexHeader>(0);
      
      _entrySize = Unsafe.SizeOf<KeyedIndexEntry<TKey>>();
   }

   public IndexSearchResult<uint> Search(BTreeSearchQuery<TKey> query)
   {
      return query.Type switch
      {
         BTreeSearchQueryType.ExactMatch => ExactMatchSearch(query),
         BTreeSearchQueryType.Between => RangeSearch(query, query.Keys[0], query.Keys[1]),
         BTreeSearchQueryType.GreaterThan => RangeSearch(query, query.Keys[0], null),
         BTreeSearchQueryType.LessThan => RangeSearch(query, null, query.Keys[0]),
         _ => new IndexSearchResult<uint>(0)
      };
   }

   private IndexSearchResult<uint> ExactMatchSearch(BTreeSearchQuery<TKey> query)
   {
      var uniqueIds = new SortedSet<uint>();
      using var buffer = _handle.GetBuffer();

      foreach (var key in keys)
      {
         var id = FindPoint(buffer, key);
         if (id.HasValue) uniqueIds.Add(id.Value);
      }

      var result = new IndexSearchResult<uint>(uniqueIds.Count);
      var i = 0;

      foreach (var id in uniqueIds)
      {
         if (i)
         result.Span[i++] = id;
      }
      
      return result;
   }

   private IndexSearchResult<uint> RangeSearch(BTreeSearchQuery<TKey> query, TKey? start, TKey? end)
   {
      return new IndexSearchResult<uint>(0);
   }
   
   private uint? FindPoint(MmfBuffer buffer, TKey key)
   {
      long currentOffset = FindLeafPageOffset(buffer, key);
      
      var pageSpan = buffer.GetSpan<byte>(currentOffset, PageSize);
      var entryCount = BinaryPrimitives.ReadInt32LittleEndian(pageSpan[..4]);
      var entries = pageSpan.Slice(4, entryCount * _entrySize).NonPortableCast<byte, KeyedIndexEntry<TKey>>();

      var idx = BinarySearchInPage(entries, key, out bool exact);
      return exact ? entries[idx].Id : null;
   }
   
   private long GetNextLevelOffset(long currentOffset)
   {
      return currentOffset + PageSize;
   }
   
   public void Dispose()
   {
      _handle.Dispose();
   }
}
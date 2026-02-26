using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Indexes.BTree;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Beskar.CodeAnalytics.Data.Indexes.Search;

namespace Beskar.CodeAnalytics.Data.Indexes.Readers;

public sealed class BTreeIndexReader<TKey> : IDisposable
   where TKey : unmanaged, IComparable<TKey>
{
   private const int PageSize = 4096;
   
   private readonly MmfHandle _handle;
   private readonly IComparer<TKey> _comparer;
   
   private readonly long _rootOffset;

   public BTreeIndexReader(string filePath, IComparer<TKey> comparer)
   {
      _handle = new MmfHandle(filePath, writable: false);
      _comparer = comparer;
      
      using var buffer = _handle.GetBuffer();
      _rootOffset = buffer.ReadInt64LittleEndian(0);
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
      using var buffer = _handle.GetBuffer();
      
      var leafOffset = FindLeafOffset(buffer, _rootOffset, query.Keys[0]);
      if (leafOffset == -1) return new IndexSearchResult<uint>(0);
      
      var leafPage = GetLeafPage(buffer, leafOffset);
      var index = BinarySearchLeaf(leafPage.Entries, query.Keys[0]);

      if (index >= 0)
      {
         var result = new IndexSearchResult<uint>(1);
         result.Span[0] = leafPage.Entries[index].Id;
         
         return result;
      }
      
      return new IndexSearchResult<uint>(0);
   }

   private IndexSearchResult<uint> RangeSearch(BTreeSearchQuery<TKey> query, TKey? start, TKey? end)
   {
      return new IndexSearchResult<uint>(0);
   }

   private long FindLeafOffset(MmfBuffer buffer, long currentOffset, TKey key)
   {
      while (true)
      {
         ref var header = ref buffer.GetRef<BTreePageHeader>(currentOffset);
         
         if (header.Type == BTreePageType.Leaf)
            return currentOffset;

         var branchPage = GetBranchPage(buffer, currentOffset, header.ItemCount);
         var index = BinarySearchBranch(branchPage, key);
         
         // In a bulk-loaded tree, the key at 'index' is the MaxKey of that subtree.
         // If key <= MaxKey, we descend into that child.
         if (index < branchPage.Length)
         {
            currentOffset = branchPage[index].PageOffset;
         }
         else
         {
            // Key is greater than the maximum key in this entire branch
            return -1; 
         }
      }
   }
   
   private int BinarySearchBranch(scoped in Span<BTreeEntry<TKey>> entries, TKey key)
   {
      var low = 0;
      var high = entries.Length - 1;

      while (low <= high)
      {
         var mid = low + ((high - low) >> 1);
         var cmp = _comparer.Compare(entries[mid].Key, key);

         if (cmp >= 0) high = mid - 1;
         else low = mid + 1;
      }
      
      return low;
   }
   
   private int BinarySearchLeaf(scoped in Span<KeyedIndexEntry<TKey>> entries, TKey key)
   {
      var low = 0;
      var high = entries.Length - 1;

      while (low <= high)
      {
         var mid = low + ((high - low) >> 1);
         var cmp = _comparer.Compare(entries[mid].Key, key);

         if (cmp == 0) return mid;
         if (cmp < 0) low = mid + 1;
         else high = mid - 1;
      }
      return -1;
   }
   
   private Span<BTreeEntry<TKey>> GetBranchPage(MmfBuffer buffer, long offset, int count)
   {
      var headerSize = Unsafe.SizeOf<BTreePageHeader>();
      return buffer.GetSpan<BTreeEntry<TKey>>(offset + headerSize, count);
   }
   
   private LeafPageResult GetLeafPage(MmfBuffer buffer, long offset)
   {
      ref var header = ref buffer.GetRef<BTreePageHeader>(offset);
      
      var headerSize = Unsafe.SizeOf<BTreePageHeader>();
      var entries = buffer.GetSpan<KeyedIndexEntry<TKey>>(offset + headerSize, header.ItemCount);
      
      return new LeafPageResult()
      {
         Header = header,
         Entries = entries
      };
   }
   
   public void Dispose()
   {
      _handle.Dispose();
   }

   private ref struct LeafPageResult
   {
      public BTreePageHeader Header;
      public Span<KeyedIndexEntry<TKey>> Entries;
   }
}
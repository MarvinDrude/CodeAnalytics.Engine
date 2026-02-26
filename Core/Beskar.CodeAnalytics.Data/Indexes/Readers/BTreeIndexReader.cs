using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Indexes.BTree;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Beskar.CodeAnalytics.Data.Indexes.Search;
using Me.Memory.Buffers;

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
      var key = query.Keys[0];
      using var buffer = _handle.GetBuffer();
      
      var leafOffset = FindLeafOffset(buffer, _rootOffset, query.Keys[0]);
      if (leafOffset == -1) return new IndexSearchResult<uint>(0);

      using var builder = new ArrayBuilder<uint>(32);

      var currentOffset = leafOffset;
      while (currentOffset != 0)
      {
         ref var header = ref buffer.GetRef<BTreePageHeader>(currentOffset);
         var entries = buffer.GetSpan<KeyedIndexEntry<TKey>>(
            currentOffset + Unsafe.SizeOf<BTreePageHeader>(), 
            header.ItemCount);
         
         var startIdx = FindFirstIndex(entries, key);
         if (startIdx == -1) break;
         
         for (var i = startIdx; i < entries.Length; i++)
         {
            if (_comparer.Compare(entries[i].Key, key) == 0)
            {
               builder.Add(entries[i].Id);
               continue;
            }

            // changed value, abort
            break;
         }
         
         currentOffset = header.NextPageOffset;
      }
      
      var result = new IndexSearchResult<uint>(builder.WrittenSpan.Length);
      if (builder.WrittenSpan.Length > 0)
      {
         builder.WrittenSpan.CopyTo(result.Span);
      }
      
      return result;
   }

   private IndexSearchResult<uint> RangeSearch(BTreeSearchQuery<TKey> query, TKey? start, TKey? end)
   {
      using var builder = new ArrayBuilder<uint>(32);
      using var buffer = _handle.GetBuffer();
      
      var currentOffset = start.HasValue 
         ? FindStartingLeafOffset(buffer, _rootOffset, start.Value) 
         : FindLeftmostLeaf(buffer, _rootOffset);
      
      while (currentOffset != 0)
      {
         ref var header = ref buffer.GetRef<BTreePageHeader>(currentOffset);
         var entries = buffer.GetSpan<KeyedIndexEntry<TKey>>(
            currentOffset + Unsafe.SizeOf<BTreePageHeader>(), 
            header.ItemCount);

         var i = 0;
         if (start.HasValue)
         {
            i = FindFirstIndexGreaterOrEqual(entries, start.Value);
            if (i == -1) 
            {
               currentOffset = header.NextPageOffset;
               continue;
            }
         }

         var isFinished = false;
         for (; i < entries.Length; i++)
         {
            if (end.HasValue && _comparer.Compare(entries[i].Key, end.Value) > 0)
            {
               isFinished = true;
               break;
            }
         
            builder.Add(entries[i].Id);
         }

         if (isFinished) break;
         currentOffset = header.NextPageOffset;
      }
      
      var result = new IndexSearchResult<uint>(builder.WrittenSpan.Length);
      if (builder.WrittenSpan.Length > 0)
      {
         builder.WrittenSpan.CopyTo(result.Span);
      }
      
      return result;
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
   
   private int FindFirstIndex(scoped in Span<KeyedIndexEntry<TKey>> entries, TKey key)
   {
      var low = 0;
      var high = entries.Length - 1;
      var result = -1;

      while (low <= high)
      {
         var mid = low + ((high - low) >> 1);
         var cmp = _comparer.Compare(entries[mid].Key, key);

         if (cmp == 0)
         {
            result = mid;
            high = mid - 1; // Keep looking left to find the absolute first
         }
         else if (cmp < 0) low = mid + 1;
         else high = mid - 1;
      }
      
      return result;
   }
   
   private Span<BTreeEntry<TKey>> GetBranchPage(MmfBuffer buffer, long offset, int count)
   {
      var headerSize = Unsafe.SizeOf<BTreePageHeader>();
      return buffer.GetSpan<BTreeEntry<TKey>>(offset + headerSize, count);
   }
   
   private long FindStartingLeafOffset(MmfBuffer buffer, long currentOffset, TKey key)
   {
      while (true)
      {
         ref var header = ref buffer.GetRef<BTreePageHeader>(currentOffset);
      
         if (header.Type == BTreePageType.Leaf)
            return currentOffset;

         var branchEntries = buffer.GetSpan<BTreeEntry<TKey>>(
            currentOffset + Unsafe.SizeOf<BTreePageHeader>(), 
            header.ItemCount);

         var index = BinarySearchBranch(branchEntries, key);
      
         if (index < branchEntries.Length)
            currentOffset = branchEntries[index].PageOffset;
         else
            return -1;
      }
   }
   
   private long FindLeftmostLeaf(MmfBuffer buffer, long currentOffset)
   {
      while (true)
      {
         ref var header = ref buffer.GetRef<BTreePageHeader>(currentOffset);
         if (header.Type == BTreePageType.Leaf) return currentOffset;
         
         var firstEntry = buffer.GetRef<BTreeEntry<TKey>>(currentOffset + Unsafe.SizeOf<BTreePageHeader>());
         currentOffset = firstEntry.PageOffset;
      }
   }

   private int FindFirstIndexGreaterOrEqual(scoped in Span<KeyedIndexEntry<TKey>> entries, TKey key)
   {
      var low = 0;
      var high = entries.Length - 1;
      var result = -1;

      while (low <= high)
      {
         var mid = low + ((high - low) >> 1);
         var cmp = _comparer.Compare(entries[mid].Key, key);

         if (cmp >= 0)
         {
            result = mid;
            high = mid - 1; // Look further left
         }
         else
         {
            low = mid + 1;
         }
      }
      return result;
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
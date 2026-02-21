using System.Collections;
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Extensions;
using Beskar.CodeAnalytics.Data.Files;
using Beskar.CodeAnalytics.Data.Indexes.Builders;
using Beskar.CodeAnalytics.Data.Indexes.Intermediate;
using Beskar.CodeAnalytics.Data.Indexes.Models;
using Beskar.CodeAnalytics.Data.Indexes.Search;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Data.Indexes.Readers;

public sealed class NGramIndexReader : IDisposable
{
   private readonly MmfHandle _handle;
   private readonly IndexHeader _header;

   private readonly int _dictionaryCount;

   public NGramIndexReader(string filePath)
   {
      _handle = new MmfHandle(filePath, writable: false);
      
      using var buffer = _handle.GetBuffer();
      _header = buffer.GetRef<IndexHeader>(0);
      
      _dictionaryCount = (int)(float)(_header.DataOffset - _header.DictionaryOffset) / Unsafe.SizeOf<DictionaryEntry<NGram3>>();
   }

   public IndexSearchResult<uint> Search(NGramSearchQuery query)
   {
      if (query.Text is { Length: 0 }) return new IndexSearchResult<uint>(0);
      
      var processingString = CreateProcessingString(query);
      var queryGrams = NGramHelper.CreateNGrams<NGram3>(processingString.AsSpan(), 0, 3, false).AsSpan();
      
      using var buffer = _handle.GetBuffer();
      var dictionary = buffer.GetSpan<DictionaryEntry<NGram3>>((long)_header.DictionaryOffset, _dictionaryCount);
      using var results = queryGrams.Length < 128
         ? new SpanOwner<(long Offset, int Count)>(stackalloc (long, int)[queryGrams.Length])
         : new SpanOwner<(long Offset, int Count)>(queryGrams.Length);
      
      for (var i = 0; i < queryGrams.Length; i++)
      {
         ref var ngram = ref queryGrams[i];
         
         var index = BinarySearchDictionary(dictionary, ngram.Key);
         if (index == -1) return new IndexSearchResult<uint>(0);

         ref var dict = ref dictionary[index];
         results.Span[i] = ((long)(_header.DataOffset + dict.Offset), (int)(dict.Count));
      }
      
      results.Span.Sort(static (x, y) => x.Count.CompareTo(y.Count));
      
      var initialCount = results.Span[0].Count;
      using var shared = new MemoryOwner<uint>(initialCount);

      var sourceSpan = buffer.GetSpan<uint>(results.Span[0].Offset, initialCount);
      var currentCount = initialCount;
      
      sourceSpan.CopyTo(shared.Span);

      for (var e = 1; e < results.Span.Length; e++)
      {
         ref var current = ref results.Span[e];
         var nextSpan = buffer.GetSpan<uint>(current.Offset, current.Count);

         currentCount = shared.Span[..currentCount].IntersectInPlace(nextSpan);
         if (currentCount == 0) break;
      }

      var result = new IndexSearchResult<uint>(currentCount);
      if (currentCount == 0) return result;
      
      shared.Span[..currentCount].CopyTo(result.Span);
      return result;
   }

   private int BinarySearchDictionary(
      scoped in Span<DictionaryEntry<NGram3>> dictionary,
      scoped in NGram3 gram)
   {
      var low = 0;
      var high = dictionary.Length - 1;

      while (low <= high)
      {
         var mid = low + (high - low) / 2;
         var cmp = _comparer.Compare(dictionary[mid].Key, gram);

         if (cmp == 0) return mid;
         if (cmp < 0) low = mid + 1;
         else high = mid - 1;
      }

      return -1;
   }

   public void Dispose()
   {
      _handle.Dispose();
   }

   private static string CreateProcessingString(NGramSearchQuery query)
   {
      return query.QueryType switch
      {
         NGramSearchQueryType.StartsWith => "$$" + query.Text,
         NGramSearchQueryType.EndsWith => query.Text + "$$",
         _ => query.Text
      };
   }
   
   private static readonly Comparer<NGram3> _comparer = Comparer<NGram3>.Create(
      static (x, y) => x.MaterializedString.CompareTo(y.MaterializedString, StringComparison.Ordinal));
}
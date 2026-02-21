using System.Buffers;

namespace Beskar.CodeAnalytics.Data.Indexes.Models;

public readonly ref struct IndexSearchResult<TEntry> : IDisposable
{
   public Span<TEntry> Span { get; }
   
   private readonly TEntry[]? _entries;

   public IndexSearchResult(int length)
   {
      if (length <= 0)
      {
         Span = Span<TEntry>.Empty;
      }
      else
      {
         _entries = ArrayPool<TEntry>.Shared.Rent(length);
         Span = _entries.AsSpan(0, length);
      }
   }
   
   public void Dispose()
   {
      if (_entries is not null) 
         ArrayPool<TEntry>.Shared.Return(_entries);
   }
}
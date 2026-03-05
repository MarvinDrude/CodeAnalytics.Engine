
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;

namespace Beskar.CodeAnalytics.Data.Metadata.Readers;

public sealed class SpecReaderCache : IDisposable
{
   private readonly ConcurrentDictionary<FileId, Lazy<ISpecFileReader>> _cache = [];

   public SpecFileReader<TSpec> GetReader<TSpec>(FileId fileId, string filePath, IComparer<TSpec> comparer)
      where TSpec : unmanaged, ISpec
   {
      if (_cache.TryGetValue(fileId, out var lazyValue))
      {
         var value = lazyValue.Value;
         return Unsafe.As<ISpecFileReader, SpecFileReader<TSpec>>(ref value);
      }
      
      var created = _cache.GetOrAdd(fileId, _ => new Lazy<ISpecFileReader>(
         () => new SpecFileReader<TSpec>(filePath, comparer))).Value;
      return Unsafe.As<ISpecFileReader, SpecFileReader<TSpec>>(ref created);
   }

   public void Dispose()
   {
      foreach (var lazyValue in _cache.Values)
         lazyValue.Value.Dispose();
   }
}
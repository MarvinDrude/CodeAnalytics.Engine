using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Indexes;
using Beskar.CodeAnalytics.Data.Indexes.Builders;
using Beskar.CodeAnalytics.Data.Indexes.Models;

namespace Beskar.CodeAnalytics.Data.Indexes;

public sealed class IndexBaker<TEntity, TKey>
   where TEntity : unmanaged, ISpec
   where TKey : unmanaged, IComparable<TKey>
{
   private readonly Func<TEntity, TKey> _selector;
   private readonly IndexType _indexType;
   private readonly string _name;

   private readonly IComparer<KeyedIndexEntry<TKey>>? _comparer;
   
   public IndexBaker(Func<TEntity, TKey> selector, IndexType indexType, string name,
      IComparer<KeyedIndexEntry<TKey>>? comparer = null)
   {
      _selector = selector;
      _indexType = indexType;
      _comparer = comparer;
      
      _name = $"{name}_{indexType}".ToLowerInvariant();
   }

   public void Bake(BakeContext context)
   {
      switch (_indexType)
      {
         case IndexType.NGram:
            if (_selector is Func<TEntity, StringFileView> stringSelector)
            {
               new NGramIndexBuilder<TEntity>(
                  context, 
                  TEntity.FileId, 
                  stringSelector, 
                  _name
               ).Build();
            }
            else
            {
               throw new InvalidOperationException(
                  $"IndexType.NGram requires a selector of type Func<{typeof(TEntity).Name}, StringFileView>, " +
                  $"but found {typeof(TKey).Name} instead."
               );
            }
            break;
         case IndexType.StaticWideBTree:
            if (_comparer is null)
            {
               throw new InvalidOperationException();
            }
            new BTreeIndexBuilder<TEntity, TKey>(
               context, 
               TEntity.FileId, 
               _selector, 
               _name, 
               _comparer)
               .Build();
            break;
      }
   }
}
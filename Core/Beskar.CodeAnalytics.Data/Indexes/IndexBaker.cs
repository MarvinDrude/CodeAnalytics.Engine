using Beskar.CodeAnalytics.Data.Bake.Models;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Indexes;
using Beskar.CodeAnalytics.Data.Indexes.Builders;

namespace Beskar.CodeAnalytics.Data.Indexes;

public sealed class IndexBaker<TEntity, TKey>
   where TEntity : unmanaged, ISpec
{
   private readonly Func<TEntity, TKey> _selector;
   private readonly IndexType _indexType;
   private readonly string _name;
   
   public IndexBaker(Func<TEntity, TKey> selector, IndexType indexType, string name)
   {
      _selector = selector;
      _indexType = indexType;
      
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
      }
   }
}
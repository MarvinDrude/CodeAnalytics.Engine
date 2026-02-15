using Beskar.CodeAnalytics.Data.Enums.Indexes;

namespace Beskar.CodeAnalytics.Data.Indexes;

public sealed class IndexBaker<TEntity, TKey>
   where TEntity : unmanaged
{
   private readonly Func<TEntity, TKey> _selector;
   private readonly IndexType _indexType;
   private readonly string _name;
   
   public IndexBaker(Func<TEntity, TKey> selector, IndexType indexType, string name)
   {
      _selector = selector;
      _indexType = indexType;
      _name = name;
   }
   
   
}
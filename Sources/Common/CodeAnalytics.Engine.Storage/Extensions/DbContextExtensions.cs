using CodeAnalytics.Engine.Storage.Builders;
using CodeAnalytics.Engine.Storage.Builders.Contexts;
using CodeAnalytics.Engine.Storage.Interfaces.Builders;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Extensions;

public static class DbContextExtensions
{
   extension<T>(T context)
      where T : DbContext
   {
      public void Detach<TEntity>(TEntity entity)
         where TEntity : class
      {
         context.Entry(entity).State = EntityState.Detached;
      }
      
      public DbAttachContext<T> AttachContext(params object[] entities)
      {
         return new DbAttachContext<T>(context, entities);
      }

      public IEntityUpsertBuilder<T, TEntity> UpdateOrCreate<TEntity>(DbSet<TEntity> dbSet)
         where TEntity : class
      {
         return new EntityUpsertBuilder<T, TEntity>(context, dbSet);
      }

      public T EnableTracking()
      {
         context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
         return context;
      }
   }
}
using CodeAnalytics.Engine.Storage.Builders;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Extensions;

public static class DbContextExtensions
{
   extension<T>(T context)
      where T : DbContext
   {
      public DbGetOrCreateOrUpdateBuilder<TEntity> GetOrCreate<TEntity>(DbSet<TEntity> dbSet)
         where TEntity : class
      {
         return new DbGetOrCreateOrUpdateBuilder<TEntity>(context, dbSet);
      }
   }
}
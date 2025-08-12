using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CodeAnalytics.Engine.Extensions.Database;

public static class DbContextExtensions
{
   extension<T>(T context)
      where T : DbContext
   {
      public async Task<TEntity> GetOrInsert<TEntity>(
         DbSet<TEntity> dbSet, Func<TEntity> factory, 
         Expression<Func<TEntity, bool>> selector, 
         CancellationToken ct = default)
         where TEntity : class
      {
         var existing = await dbSet.AsNoTracking()
            .SingleOrDefaultAsync(selector, ct);
         if (existing is not null)
         {
            return existing;
         }

         var entity = factory();
         await context.AddAsync(entity, ct);

         try
         {
            await context.SaveChangesAsync(ct);
            return entity;
         }
         catch (DbUpdateException)
         {
            existing = await dbSet.AsNoTracking()
               .SingleOrDefaultAsync(selector, ct);
            if (existing is null) throw;
            
            return existing;
         }
         finally
         {
            context.Entry(entity).State = EntityState.Detached;
         }
      }

      public async Task<TEntity> ExecuteUpdateOrInsertAndUpdate<TEntity>(
         DbSet<TEntity> dbSet, Func<TEntity> factory, 
         Expression<Func<TEntity, bool>> selector, 
         Action<UpdateSettersBuilder<TEntity>> update,
         CancellationToken ct = default)
         where TEntity : class
      {
         var affected = await dbSet.Where(selector).ExecuteUpdateAsync(update, ct);
         if (affected > 0)
         {
            return await dbSet.AsNoTracking().SingleAsync(selector, ct);
         }

         var entity = factory();
         await context.AddAsync(entity, ct);

         try
         {

         }
         catch (DbUpdateException)
         {
            
         }
      }
   }
}
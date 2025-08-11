using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Extensions.Database;

public static class DbContextExtensions
{
   extension<T>(T context)
      where T : DbContext
   {
      public async Task<TEntity> GetOrInsert<TEntity>(
         DbSet<TEntity> dbSet, TEntity entity, 
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
   }
}
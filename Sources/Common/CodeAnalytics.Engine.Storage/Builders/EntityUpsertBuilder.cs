using System.Linq.Expressions;
using CodeAnalytics.Engine.Storage.Interfaces.Builders;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Builders;

internal sealed class EntityUpsertBuilder<TContext, TEntity> : IEntityUpsertBuilder<TContext, TEntity>
   where TEntity : class
   where TContext : DbContext
{
   private readonly TContext _context;
   private readonly DbSet<TEntity> _dbSet;

   private Expression<Func<TEntity, bool>>? _predicate;
   private Func<IQueryable<TEntity>, IQueryable<TEntity>>? _include;
   private Func<TEntity>? _factory;
   private Func<TContext, TEntity, CancellationToken, Task<TEntity>>? _updater;
   
   internal EntityUpsertBuilder(TContext context, DbSet<TEntity> dbSet)
   {
      _context = context;
      _dbSet = dbSet;
   }
   
   public IEntityUpsertBuilder<TContext, TEntity> Match(Expression<Func<TEntity, bool>> predicate)
   {
      _predicate = predicate;
      return this;
   }

   public IEntityUpsertBuilder<TContext, TEntity> Include(Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
   {
      _include = _include is null
         ? include
         : (q) => include(_include(q));
      
      return this;
   }

   public IEntityUpsertBuilder<TContext, TEntity> OnCreate(Func<TEntity> factory)
   {
      _factory = factory;
      return this;
   }

   public IEntityUpsertBuilder<TContext, TEntity> OnUpdate(Func<TContext, TEntity, CancellationToken, Task<TEntity>> updater)
   {
      _updater = updater;
      return this;
   }

   public async Task<TEntity> ExecuteAsync(TContext context, CancellationToken ct = default)
   {
      if (_predicate is null) throw new InvalidOperationException("No predicate passed.");
      if (_factory is null) throw new InvalidOperationException("No create passed.");
      
      if (await TryQuery(ct) is { } existing)
      {
         if (_updater is null) return existing;
         return await RunUpdate(existing, ct);
      }

      var entity = _factory();
      await _context.AddAsync(entity, ct);

      try
      {
         await _context.SaveChangesAsync(ct);
         return entity;
      }
      catch (DbUpdateException)
      {
         var raced = await TryQuery(ct);
         if (raced is null) throw;

         if (_updater is null) return raced;
         return await RunUpdate(raced, ct);
      }
      finally
      {
         _context.Entry(entity).State = EntityState.Detached;
      }
   }
   
   private async Task<TEntity> RunUpdate(TEntity entity, CancellationToken ct = default)
   {
      if (_updater is null) throw new InvalidOperationException("No update passed.");
      var updated = await _updater(_context, entity, ct);
      
      return updated;
   }
   
   private async Task<TEntity?> TryQuery(CancellationToken ct = default)
   {
      if (_predicate is null) return null;
      
      IQueryable<TEntity> query = _dbSet;
      if (_include is not null) query = _include(query);

      return await query
         .AsNoTracking()
         .Where(_predicate)
         .SingleOrDefaultAsync(ct);
   }
}
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Builders;

public sealed class DbGetOrCreateOrUpdateBuilder<TEntity>
   where TEntity : class
{
   private readonly DbContext _context;
   private readonly DbSet<TEntity> _dbSet;

   private Func<IQueryable<TEntity>, IQueryable<TEntity>>? _include;
   private Expression<Func<TEntity, bool>>? _predicate;
   
   private Func<TEntity>? _onCreate;
   private Func<DbContext, TEntity, Task<TEntity>>? _onUpdate;

   internal DbGetOrCreateOrUpdateBuilder(DbContext context, DbSet<TEntity> dbSet)
   {
      _context = context;
      _dbSet = dbSet;
   }

   public DbGetOrCreateOrUpdateBuilder<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
   {
      _predicate = predicate;
      return this;
   }

   public DbGetOrCreateOrUpdateBuilder<TEntity> Including(
      Func<IQueryable<TEntity>, IQueryable<TEntity>> includer)
   {
      _include = _include is null
         ? includer
         : (q) => includer(_include(q));
      return this;
   }

   public DbGetOrCreateOrUpdateBuilder<TEntity> OnUpdate(Func<DbContext, TEntity, Task<TEntity>> update)
   {
      _onUpdate = update;
      return this;
   }

   public DbGetOrCreateOrUpdateBuilder<TEntity> OnCreate(Func<TEntity> create)
   {
      _onCreate = create;
      return this;
   }

   public async Task<TEntity> Execute(CancellationToken ct = default)
   {
      if (_predicate is null) throw new InvalidOperationException("No predicate passed.");
      if (_onCreate is null) throw new InvalidOperationException("No create passed.");

      if (await TryQuery(ct) is { } existing)
      {
         if (_onUpdate is null) return existing;
         return await RunUpdate(existing, ct);
      }

      var entity = _onCreate();
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

         if (_onUpdate is null) return raced;
         return await RunUpdate(raced, ct);
      }
      finally
      {
         _context.Entry(entity).State = EntityState.Detached;
      }
   }

   private async Task<TEntity> RunUpdate(TEntity entity, CancellationToken ct = default)
   {
      if (_onUpdate is null) throw new InvalidOperationException("No update passed.");
      var updated = await _onUpdate(_context, entity);
      
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
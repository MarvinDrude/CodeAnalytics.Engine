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
   
   private Func<TEntity, TEntity>? _onUpdate;
   private Func<TEntity>? _onCreate;

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

   public DbGetOrCreateOrUpdateBuilder<TEntity> Include(
      Func<IQueryable<TEntity>, IQueryable<TEntity>> includer)
   {
      _include = _include is null
         ? includer
         : (q) => includer(_include(q));
      return this;
   }

   public DbGetOrCreateOrUpdateBuilder<TEntity> OnUpdate(Func<TEntity, TEntity> update)
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
         
         var updated = _onUpdate(existing);
         _context.Entry(updated).State = EntityState.Modified;
            
         await _context.SaveChangesAsync(ct);
         _context.Entry(updated).State = EntityState.Detached;
         return updated;
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
         
         var updated = _onUpdate(raced);
         _context.Entry(updated).State = EntityState.Modified;
            
         await _context.SaveChangesAsync(ct);
         _context.Entry(updated).State = EntityState.Detached;
            
         return updated;
      }
      finally
      {
         _context.Entry(entity).State = EntityState.Detached;
      }
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
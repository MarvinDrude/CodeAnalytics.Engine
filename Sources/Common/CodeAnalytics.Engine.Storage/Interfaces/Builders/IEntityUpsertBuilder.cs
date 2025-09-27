using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Interfaces.Builders;

public interface IEntityUpsertBuilder<TContext, TEntity>
   where TEntity : class
   where TContext : DbContext
{
   IEntityUpsertBuilder<TContext, TEntity> Match(Expression<Func<TEntity, bool>> predicate);
   IEntityUpsertBuilder<TContext, TEntity> Include(Func<IQueryable<TEntity>, IQueryable<TEntity>> include);
   IEntityUpsertBuilder<TContext, TEntity> OnCreate(Func<TEntity> factory);
   IEntityUpsertBuilder<TContext, TEntity> OnUpdate(Func<TContext, TEntity, CancellationToken, Task<TEntity>> updater);

   Task<TEntity> ExecuteAsync(TContext context, CancellationToken ct = default);
}
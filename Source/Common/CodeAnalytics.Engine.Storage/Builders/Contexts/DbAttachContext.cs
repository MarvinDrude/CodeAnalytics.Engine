using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Builders.Contexts;

public sealed class DbAttachContext<TContext> : IDisposable
   where TContext : DbContext
{
   private readonly TContext _context;
   private readonly object[] _entities;
   
   public DbAttachContext(
      TContext context,
      params object[] entities)
   {
      _context = context;
      _entities = entities;
      
      context.AttachRange(entities);
   }

   public void Dispose()
   {
      foreach (var entity in _entities)
         _context.Entry(entity).State = EntityState.Detached;
   }
}
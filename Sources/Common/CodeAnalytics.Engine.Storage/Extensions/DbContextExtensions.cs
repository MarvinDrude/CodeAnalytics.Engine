using CodeAnalytics.Engine.Storage.Builders.Contexts;
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
   }
}
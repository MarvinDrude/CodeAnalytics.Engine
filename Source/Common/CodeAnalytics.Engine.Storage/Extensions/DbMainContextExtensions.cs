using CodeAnalytics.Engine.Storage.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Extensions;

public static class DbMainContextExtensions
{
   extension(DbMainContext context)
   {
      public Task<long> GetSymbolId(string hashId)
      {
         return context.Symbols
            .Where(x => x.UniqueIdHash == hashId)
            .Select(x => x.Id)
            .SingleOrDefaultAsync();
      }
   }
}
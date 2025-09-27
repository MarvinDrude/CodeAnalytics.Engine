using CodeAnalytics.Engine.Storage.Common;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Collectors.Extensions.Database;

public static class DbMainContextExtensions
{
   extension(DbMainContext context)
   {
      public Task<DbSymbolId> GetSymbolId(string hashId)
      {
         return context.Symbols
            .Where(x => x.UniqueIdHash == hashId)
            .Select(x => x.Id)
            .SingleOrDefaultAsync();
      }
   }
}
using CodeAnalytics.Engine.Storage.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeAnalytics.Engine.Storage.Extensions;

public static class ServiceCollectionExtensions
{
   extension(IServiceCollection collection)
   {
      public DatabaseOptions AddAndGetDatabaseOptions(IConfiguration configuration)
      {
         var instance = configuration
            .GetSection(DatabaseOptions.Prefix)
            .Get<DatabaseOptions>()
            ?? new DatabaseOptions();

         collection.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.Prefix));

         return instance;
      }
   }
}
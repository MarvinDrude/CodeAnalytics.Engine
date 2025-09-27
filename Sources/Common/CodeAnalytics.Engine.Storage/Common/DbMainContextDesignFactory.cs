using CodeAnalytics.Engine.Storage.Extensions;
using CodeAnalytics.Engine.Storage.Options;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeAnalytics.Engine.Storage.Common;

public sealed class DbMainContextDesignFactory : IDesignTimeDbContextFactory<DbMainContext>
{
   public DbMainContext CreateDbContext(string[] args)
   {
      var configBuilder = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
         .AddEnvironmentVariables(prefix: "CA_")
         .AddCommandLine(args);
      var configuration = configBuilder.Build();
      
      var services = new ServiceCollection();
      services.AddAndGetDatabaseOptions(configuration);
      services.AddLogging();
      
      var serviceProvider = services.BuildServiceProvider();

      return new DbMainContext(
         serviceProvider.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>(),
         serviceProvider.GetRequiredService<ILogger<DbMainContext>>(),
         serviceProvider.GetRequiredService<ILoggerFactory>());
   }
}
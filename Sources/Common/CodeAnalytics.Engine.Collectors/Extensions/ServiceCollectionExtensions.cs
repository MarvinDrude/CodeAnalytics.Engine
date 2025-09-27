using CodeAnalytics.Engine.Collectors.Common;
using CodeAnalytics.Engine.Collectors.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeAnalytics.Engine.Collectors.Extensions;

public static class ServiceCollectionExtensions
{
   extension(IServiceCollection collection)
   {
      public CollectorOptions AddAndGetCollectorOptions(IConfiguration configuration)
      {
         var instance = configuration
            .GetSection(CollectorOptions.Prefix)
            .Get<CollectorOptions>()
            ?? new CollectorOptions();

         collection.AddOptions<CollectorOptions>()
            .Bind(configuration.GetSection(CollectorOptions.Prefix));

         return instance;
      }

      public IServiceCollection AddCollectorServices()
      {
         return collection
            .AddTransient<SolutionCollector>()
            .AddTransient<ProjectCollector>();
      }
   }
}
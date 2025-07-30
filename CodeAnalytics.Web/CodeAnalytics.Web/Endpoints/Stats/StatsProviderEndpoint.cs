using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;
using CodeAnalytics.Engine.Contracts.Pipelines.Models;
using CodeAnalytics.Web.Common.Constants.Stats;
using Microsoft.AspNetCore.Mvc;

namespace CodeAnalytics.Web.Endpoints.Stats;

public static class StatsProviderEndpoint
{
   public static void Map(IEndpointRouteBuilder endpoints)
   {
      endpoints.MapPost(StatsApiConstants.PathGetStats, GetStats);
   }

   private static async Task<string> GetStats(
      [FromQuery] string type,
      [FromBody] PipelineParameters parameters,
      IServiceProvider provider)
   {
      var pipeProvider = provider.GetRequiredKeyedService<IPipelineProvider>(type);
      return await pipeProvider.RunRawString(parameters);
   }
}
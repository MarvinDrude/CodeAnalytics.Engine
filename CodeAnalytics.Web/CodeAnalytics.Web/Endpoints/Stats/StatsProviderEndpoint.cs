using CodeAnalytics.Web.Common.Constants.Stats;

namespace CodeAnalytics.Web.Endpoints.Stats;

public static class StatsProviderEndpoint
{
   public static void Map(IEndpointRouteBuilder endpoints)
   {
      endpoints.MapGet(StatsApiConstants.PathGetStats, GetStats);
   }

   private static async Task GetStats()
   {
      
   }
}
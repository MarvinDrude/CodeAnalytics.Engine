using CodeAnalytics.Web.Common.Constants.Data;
using CodeAnalytics.Web.Common.Services.Data;
using Microsoft.AspNetCore.Mvc;

namespace CodeAnalytics.Web.Endpoints.Data;

public class GetOccurrenceStringsEndpoint
{
   public static void Map(IEndpointRouteBuilder endpoints)
   {
      endpoints.MapGet(DataApiConstants.PathGetOccurrenceStrings, GetOccurenceStrings);
   }

   private static async Task<Dictionary<int, string>?> GetOccurenceStrings(
      [FromQuery] int rawNodeId, IOccurrenceService service)
   {
      var occurrences = await service.GetOccurrenceStrings(rawNodeId);
      return occurrences;
   }
}
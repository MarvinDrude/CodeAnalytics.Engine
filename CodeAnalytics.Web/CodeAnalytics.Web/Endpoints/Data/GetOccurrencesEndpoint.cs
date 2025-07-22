using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Engine.Serialization;
using CodeAnalytics.Engine.Serialization.Occurrence;
using CodeAnalytics.Web.Common.Constants.Data;
using CodeAnalytics.Web.Common.Services.Data;
using Microsoft.AspNetCore.Mvc;

namespace CodeAnalytics.Web.Endpoints.Data;

public class GetOccurrencesEndpoint
{
   public static void Map(IEndpointRouteBuilder endpoints)
   {
      endpoints.MapGet(DataApiConstants.PathGetOccurrences, GetOccurences);
   }

   private static async Task<IResult> GetOccurences(
      [FromQuery] int rawNodeId, IOccurrenceService service)
   {
      var occurrences = await service.GetOccurrences(rawNodeId);
      if (occurrences == null)
      {
         return Results.File([], "application/octet-stream");
      }

      var bytes = Serializer<GlobalOccurrence, GlobalOccurrenceSerializer>
         .ToMemory(ref occurrences);
      return Results.File(bytes.ToArray(), "application/octet-stream");
   }
}
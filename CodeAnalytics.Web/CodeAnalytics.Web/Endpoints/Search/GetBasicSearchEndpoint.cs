using CodeAnalytics.Engine.Serialization;
using CodeAnalytics.Web.Common.Constants.Search;
using CodeAnalytics.Web.Common.Models.Search;
using CodeAnalytics.Web.Common.Responses.Search;
using CodeAnalytics.Web.Common.Serialization.Search;
using CodeAnalytics.Web.Common.Services.Search;
using Microsoft.AspNetCore.Mvc;

namespace CodeAnalytics.Web.Endpoints.Search;

public static class GetBasicSearchEndpoint
{
   public static void Map(IEndpointRouteBuilder endpoints)
   {
      endpoints.MapPost(SearchApiConstants.PathGetBasicSearch, GetBasicSearch);
   }
   
   private static async Task<IResult> GetBasicSearch(
      [FromBody] BasicSearchParameters parameters, ISearchService service)
   {
      var searchResponse = await service.GetBasicSearch(parameters);

      var bytes = Serializer<BasicSearchResponse, BasicSearchResponseSerializer>
         .ToMemory(ref searchResponse);
      return Results.File(bytes.ToArray(), "application/octet-stream");
   }
}
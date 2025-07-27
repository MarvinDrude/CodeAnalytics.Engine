using CodeAnalytics.Web.Common.Constants.Search;
using CodeAnalytics.Web.Common.Models.Search;
using CodeAnalytics.Web.Common.Services.Search;
using Microsoft.AspNetCore.Mvc;

namespace CodeAnalytics.Web.Endpoints.Search;

public static class GetFileSearchEndpoint
{
   public static void Map(IEndpointRouteBuilder endpoints)
   {
      endpoints.MapPost(SearchApiConstants.PathGetFileSearch, GetFileSearch);
   }
   
   private static async Task<List<ExplorerTreeItemSearchModel>> GetFileSearch(
      [FromBody] FileSearchParameters parameters, IFileSearchService service)
   {
      var result = await service.GetFileSearch(parameters);
      return result;
   }
}
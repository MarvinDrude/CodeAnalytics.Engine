using CodeAnalytics.Web.Common.Models.Search;
using CodeAnalytics.Web.Common.Responses.Search;
using CodeAnalytics.Web.Common.Services.Data;
using CodeAnalytics.Web.Common.Services.Search;

namespace CodeAnalytics.Web.Services.Search;

public sealed class ServerSearchService : ISearchService
{
   private readonly IDataService _dataService;

   public ServerSearchService(IDataService dataService)
   {
      _dataService = dataService;
   }
   
   public Task<BasicSearchResponse> GetBasicSearch(BasicSearchParameters parameters)
   {
      
   }
}
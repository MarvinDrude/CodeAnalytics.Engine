using CodeAnalytics.Web.Common.Models.Search;
using CodeAnalytics.Web.Common.Responses.Search;
using CodeAnalytics.Web.Common.Services.Search;

namespace CodeAnalytics.Web.Client.Services.Search;

public sealed class ClientSearchService : ISearchService
{
   private readonly HttpClient _client;

   public ClientSearchService(HttpClient client)
   {
      _client = client;
   }
   
   public Task<BasicSearchResponse> GetBasicSearch(BasicSearchParameters parameters)
   {
      throw new NotImplementedException();
   }
}
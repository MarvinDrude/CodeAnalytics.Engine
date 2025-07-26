using System.Net.Http.Json;
using CodeAnalytics.Engine.Serialization;
using CodeAnalytics.Web.Common.Constants.Search;
using CodeAnalytics.Web.Common.Models.Search;
using CodeAnalytics.Web.Common.Responses.Search;
using CodeAnalytics.Web.Common.Serialization.Search;
using CodeAnalytics.Web.Common.Services.Search;

namespace CodeAnalytics.Web.Client.Services.Search;

public sealed class ClientSearchService : ISearchService
{
   private readonly HttpClient _client;

   public ClientSearchService(HttpClient client)
   {
      _client = client;
   }
   
   public async Task<BasicSearchResponse> GetBasicSearch(BasicSearchParameters parameters)
   {
      const string url = SearchApiConstants.FullPathGetBasicSearch;
      var response = await _client.PostAsJsonAsync(url, parameters);

      var bytes = await response.Content.ReadAsByteArrayAsync();
      return Serializer<BasicSearchResponse, BasicSearchResponseSerializer>.FromMemory(bytes);
   }
}
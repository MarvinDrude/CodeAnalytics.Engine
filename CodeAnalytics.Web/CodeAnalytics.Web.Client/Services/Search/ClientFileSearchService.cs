using System.Net.Http.Json;
using CodeAnalytics.Web.Common.Constants.Search;
using CodeAnalytics.Web.Common.Models.Search;
using CodeAnalytics.Web.Common.Services.Search;

namespace CodeAnalytics.Web.Client.Services.Search;

public sealed class ClientFileSearchService : IFileSearchService
{
   private readonly HttpClient _client;

   public ClientFileSearchService(HttpClient client)
   {
      _client = client;
   }
   
   public async Task<List<ExplorerTreeItemSearchModel>> GetFileSearch(FileSearchParameters parameters)
   {
      const string url = SearchApiConstants.FullPathGetFileSearch;
      var response = await _client.PostAsJsonAsync(url, parameters);

      return await response.Content.ReadFromJsonAsync<List<ExplorerTreeItemSearchModel>>()
         ?? [];
   }
}
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
   
   public Task<List<ExplorerTreeItemSearchModel>> GetFileSearch(FileSearchParameters parameters)
   {
      throw new NotImplementedException();
   }
}
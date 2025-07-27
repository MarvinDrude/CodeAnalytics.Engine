using System.Net.Http.Json;
using CodeAnalytics.Web.Common.Constants.Source;
using CodeAnalytics.Web.Common.Models.Explorer;
using CodeAnalytics.Web.Common.Services.Source;

namespace CodeAnalytics.Web.Client.Services.Source;

public sealed class ClientExplorerService : IExplorerService
{
   private readonly HttpClient _client;

   public ClientExplorerService(HttpClient client)
   {
      _client = client;
   }
   
   public async Task<List<ExplorerTreeItem>> GetExplorerTreeItems()
   {
      var result = await _client.GetFromJsonAsync<List<ExplorerTreeItem>>(SourceApiConstants.FullPathGetExplorerTreeItems);
      return result ?? [];
   }

   public Task<ExplorerFlatTreeItem[]> GetFlatTreeItems()
   {
      // not supported in client side directly
      throw new NotImplementedException();
   }
}
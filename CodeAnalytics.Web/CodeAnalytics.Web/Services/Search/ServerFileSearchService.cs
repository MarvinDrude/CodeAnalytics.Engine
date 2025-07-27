using CodeAnalytics.Web.Common.Models.Search;
using CodeAnalytics.Web.Common.Services.Search;
using CodeAnalytics.Web.Common.Services.Source;

namespace CodeAnalytics.Web.Services.Search;

public sealed class ServerFileSearchService : IFileSearchService
{
   private readonly IExplorerService _explorerService;

   public ServerFileSearchService(IExplorerService explorerService)
   {
      _explorerService = explorerService;
   }
   
   public async Task<List<ExplorerTreeItemSearchModel>> GetFileSearch(FileSearchParameters parameters)
   {
      var all = await _explorerService.GetFlatTreeItems();
      List<ExplorerTreeItemSearchModel> result = [];

      foreach (ref var item in all.AsSpan())
      {
         if (!parameters.Types.Contains(item.Type))
         {
            continue;
         }

         if (!item.Name.Contains(parameters.SearchText, StringComparison.OrdinalIgnoreCase))
         {
            continue;
         }

         result.Add(new ExplorerTreeItemSearchModel()
         {
            Item = item,
            Path = item.Path
         });
         
         if (parameters.MaxResults <= result.Count)
         {
            break;
         }
      }

      return result;
   }
}
using CodeAnalytics.Web.Common.Constants.Source;
using CodeAnalytics.Web.Common.Models.Explorer;
using CodeAnalytics.Web.Common.Services.Source;

namespace CodeAnalytics.Web.Endpoints.Source;

public class GetExplorerTreeItemsEndpoint
{
   public static void Map(IEndpointRouteBuilder endpoints)
   {
      endpoints.MapGet(SourceApiConstants.PathGetExplorerTreeItems, GetSyntaxSpansByPath);
   }

   private static async Task<List<ExplorerTreeItem>> GetSyntaxSpansByPath(
      IExplorerService service)
   {
      var result = await service.GetExplorerTreeItems();
      return result;
   }
}
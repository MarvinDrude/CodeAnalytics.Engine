using CodeAnalytics.Web.Common.Models.Explorer;

namespace CodeAnalytics.Web.Common.Services.Source;

public interface IExplorerService
{
   public Task<List<ExplorerTreeItem>> GetExplorerTreeItems();
   
   public Task<ExplorerFlatTreeItem[]> GetFlatTreeItems();
}
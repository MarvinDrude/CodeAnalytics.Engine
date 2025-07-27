using CodeAnalytics.Web.Common.Models.Explorer;

namespace CodeAnalytics.Web.Common.Models.Search;

public sealed class ExplorerTreeItemSearchModel
{
   public required ExplorerFlatTreeItem Item { get; set; }
   
   public required string[] Path { get; set; }
}
using CodeAnalytics.Web.Common.Enums.Explorer;

namespace CodeAnalytics.Web.Common.Models.Search;

public sealed class FileSearchParameters
{
   public required string SearchText  { get; set; }
   
   public required HashSet<ExplorerTreeItemType> Types { get; set; }

   public int MaxResults { get; set; } = 100;
}
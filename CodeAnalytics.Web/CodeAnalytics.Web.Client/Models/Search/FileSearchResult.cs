using CodeAnalytics.Web.Common.Models.Search;

namespace CodeAnalytics.Web.Client.Models.Search;

public sealed class FileSearchResult : SearchResult
{
   public required ExplorerTreeItemSearchModel Item { get; init; }
}
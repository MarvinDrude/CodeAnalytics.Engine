using CodeAnalytics.Web.Common.Models.Explorer;
using CodeAnalytics.Web.Common.Models.Search;

namespace CodeAnalytics.Web.Common.Services.Search;

public interface IFileSearchService
{
   public Task<List<ExplorerTreeItemSearchModel>> GetFileSearch(FileSearchParameters parameters);
}
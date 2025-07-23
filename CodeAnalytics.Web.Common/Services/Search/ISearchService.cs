using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Web.Common.Models.Search;
using CodeAnalytics.Web.Common.Responses.Search;

namespace CodeAnalytics.Web.Common.Services.Search;

public interface ISearchService
{
   public Task<BasicSearchResponse> GetBasicSearch(BasicSearchParameters parameters);
}
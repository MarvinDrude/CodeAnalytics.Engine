using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Web.Common.Models.Search;

namespace CodeAnalytics.Web.Common.Services.Search;

public interface ISearchService
{
   public Task<List<IArchetype>> GetBasicSearch(BasicSearchParameters parameters);
}
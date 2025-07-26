using CodeAnalytics.Engine.Analyze.Searchers;
using CodeAnalytics.Engine.StringResolvers.Archetypes;
using CodeAnalytics.Web.Common.Models.Search;
using CodeAnalytics.Web.Common.Responses.Search;
using CodeAnalytics.Web.Common.Services.Data;
using CodeAnalytics.Web.Common.Services.Search;

namespace CodeAnalytics.Web.Services.Search;

public sealed class ServerSearchService : ISearchService
{
   private readonly IDataService _dataService;

   public ServerSearchService(IDataService dataService)
   {
      _dataService = dataService;
   }
   
   public async Task<BasicSearchResponse> GetBasicSearch(BasicSearchParameters parameters)
   {
      var store = await _dataService.GetAnalyzeStore();
      var archSearcher = new BasicArchetypeSearcher(store, parameters.Options);
      archSearcher.Search();

      Dictionary<int, string> strings = [];
      await DynamicArchetypeStringResolver.Resolve(strings, archSearcher.Results);
      
      return new BasicSearchResponse()
      {
         MaxResults = parameters.Options.MaxResults,
         Results = archSearcher.Results,
         Strings = strings
      };
   }
}
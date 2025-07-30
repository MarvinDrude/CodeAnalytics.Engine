using CodeAnalytics.Web.Common.Constants.Data;
using CodeAnalytics.Web.Common.Constants.Search;
using CodeAnalytics.Web.Common.Constants.Source;
using CodeAnalytics.Web.Common.Constants.Stats;
using CodeAnalytics.Web.Endpoints.Data;
using CodeAnalytics.Web.Endpoints.Search;
using CodeAnalytics.Web.Endpoints.Source;
using CodeAnalytics.Web.Endpoints.Stats;

namespace CodeAnalytics.Web.Endpoints;

public static class EndpointMapper
{
   public static void Map(IEndpointRouteBuilder endpoints)
   {
      var data = endpoints.MapGroup(DataApiConstants.BasePath);

      GetOccurrencesEndpoint.Map(data);
      GetOccurrenceStringsEndpoint.Map(data);
      
      var source = endpoints.MapGroup(SourceApiConstants.BasePath);
      
      GetSourceTextSpansEndpoint.Map(source);
      GetExplorerTreeItemsEndpoint.Map(source);

      var search = endpoints.MapGroup(SearchApiConstants.BasePath);
      
      GetBasicSearchEndpoint.Map(search);
      GetFileSearchEndpoint.Map(search);

      var stats = endpoints.MapGroup(StatsApiConstants.BasePath);
      
      StatsProviderEndpoint.Map(stats);
   }
}
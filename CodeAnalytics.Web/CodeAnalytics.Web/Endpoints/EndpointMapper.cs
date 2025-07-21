using CodeAnalytics.Web.Common.Constants.Source;
using CodeAnalytics.Web.Endpoints.Source;

namespace CodeAnalytics.Web.Endpoints;

public static class EndpointMapper
{
   public static void Map(IEndpointRouteBuilder endpoints)
   {
      var source = endpoints.MapGroup(SourceApiConstants.BasePath);
      
      GetSourceTextSpansEndpoint.Map(source);
      GetExplorerTreeItemsEndpoint.Map(source);
   }
}
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.TextRendering;
using CodeAnalytics.Web.Common.Constants.Source;
using CodeAnalytics.Web.Common.Services.Source;
using Microsoft.AspNetCore.Mvc;

namespace CodeAnalytics.Web.Endpoints.Source;

public static class GetSourceTextSpansEndpoint
{
   public static void Map(IEndpointRouteBuilder endpoints)
   {
      endpoints.MapGet(SourceApiConstants.PathGetSourceSpansByPath, GetSyntaxSpansByPath);
   }

   private static async Task<Result<SyntaxSpan[], Error<string>>> GetSyntaxSpansByPath(
      [FromQuery] string path, ISourceTextService service)
   {
      return await service.GetSyntaxSpansByPath(path);
   }
}
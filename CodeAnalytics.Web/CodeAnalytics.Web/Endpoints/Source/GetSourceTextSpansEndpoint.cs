using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.TextRendering;
using CodeAnalytics.Engine.Extensions.Memory;
using CodeAnalytics.Engine.Serialization.Common;
using CodeAnalytics.Engine.Serialization.System.Collections;
using CodeAnalytics.Engine.Serialization.System.Common;
using CodeAnalytics.Engine.Serialization.TextRendering;
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

   private static async Task<IResult> GetSyntaxSpansByPath(
      [FromQuery] string path, ISourceTextService service)
   {
      var result = await service.GetSyntaxSpansByPath(path);
      var bytes = result.ToMemory<
         SyntaxSpan[], ManagedArraySerializer<SyntaxSpan, SyntaxSpanSerializer>, 
         Error<string>, ManagedErrorSerializer<string, StringSerializer>>();

      return Results.File(bytes.ToArray(), "application/octet-stream");
   }
}
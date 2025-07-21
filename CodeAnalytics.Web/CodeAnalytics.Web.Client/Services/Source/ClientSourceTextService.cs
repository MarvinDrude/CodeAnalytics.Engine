using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.TextRendering;
using CodeAnalytics.Engine.Serialization;
using CodeAnalytics.Engine.Serialization.Common;
using CodeAnalytics.Engine.Serialization.System.Collections;
using CodeAnalytics.Engine.Serialization.System.Common;
using CodeAnalytics.Engine.Serialization.TextRendering;
using CodeAnalytics.Web.Common.Constants.Source;
using CodeAnalytics.Web.Common.Services.Source;

namespace CodeAnalytics.Web.Client.Services.Source;

public sealed class ClientSourceTextService : ISourceTextService
{
   private readonly HttpClient _client;

   public ClientSourceTextService(HttpClient client)
   {
      _client = client;
   }
   
   public Task<Result<SyntaxSpan[], Error<string>>> GetSyntaxSpansByStringId(StringId stringId)
   {
      return GetSyntaxSpansByPath(stringId.ToString());
   }

   public async Task<Result<SyntaxSpan[], Error<string>>> GetSyntaxSpansByPath(string path)
   {
      var escaped = Uri.EscapeDataString(path);
      var url = $"{SourceApiConstants.FullPathGetSourceSpansByPath}?path={escaped}";

      var bytes = await _client.GetByteArrayAsync(url);

      if (bytes.Length == 0)
      {
         return new Error<string>("Unexpected error response.");
      }

      var result = Serializer<
         Result<SyntaxSpan[], Error<string>>,
         ResultSerializer<
            SyntaxSpan[], ManagedArraySerializer<SyntaxSpan, SyntaxSpanSerializer>,
            Error<string>, ManagedErrorSerializer<string, StringSerializer>>
      >.FromMemory(bytes);
      
      return result;
   }
}
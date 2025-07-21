using System.Net.Http.Json;
using CodeAnalytics.Engine.Common.Results;
using CodeAnalytics.Engine.Common.Results.Errors;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.TextRendering;
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
      
      var result = await _client.GetFromJsonAsync<Result<SyntaxSpan[], Error<string>>>(url);
      return result;
   }
}
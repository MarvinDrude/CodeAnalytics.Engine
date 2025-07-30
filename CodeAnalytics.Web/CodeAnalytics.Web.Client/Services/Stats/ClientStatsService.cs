using System.Net.Http.Json;
using System.Text.Json;
using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;
using CodeAnalytics.Engine.Contracts.Pipelines.Models;
using CodeAnalytics.Engine.Json.Extensions;
using CodeAnalytics.Web.Common.Constants.Stats;
using CodeAnalytics.Web.Common.Services.Stats;

namespace CodeAnalytics.Web.Client.Services.Stats;

public sealed class ClientStatsService : IStatsService
{
   private static readonly JsonSerializerOptions _jsonOptions = GetJsonSerializerOptions();
   private readonly HttpClient _client;

   public ClientStatsService(HttpClient client)
   {
      _client = client;
   }
   
   public async Task<TResult> GetStats<TResult, TProvider>(PipelineParameters parameters) 
      where TProvider : IPipelineProvider
   {
      var response = await _client.PostAsJsonAsync(
         $"{StatsApiConstants.FullPathGetStats}?type={TProvider.Identifier}", parameters, _jsonOptions);
      var result = await response.Content.ReadFromJsonAsync<TResult>(_jsonOptions);

      return result ?? throw new InvalidOperationException("Invalid json response.");
   }
   
   private static JsonSerializerOptions GetJsonSerializerOptions()
   {
      var options = new JsonSerializerOptions()
      {
         IncludeFields = true
      };
      options.AddConverters();
      
      return options;
   }
}
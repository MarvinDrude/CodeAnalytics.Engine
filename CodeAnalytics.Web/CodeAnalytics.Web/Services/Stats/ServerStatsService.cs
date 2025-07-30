using System.Text.Json;
using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;
using CodeAnalytics.Engine.Contracts.Pipelines.Models;
using CodeAnalytics.Engine.Json.Extensions;
using CodeAnalytics.Web.Common.Services.Stats;

namespace CodeAnalytics.Web.Services.Stats;

public sealed class ServerStatsService : IStatsService
{
   private static readonly JsonSerializerOptions _jsonOptions = GetJsonSerializerOptions();
   private readonly IServiceProvider _provider;

   public ServerStatsService(IServiceProvider provider)
   {
      _provider = provider;
   }
   
   public async Task<TResult> GetStats<TResult, TProvider>(PipelineParameters parameters) 
      where TProvider : IPipelineProvider
   {
      var provider = _provider.GetRequiredKeyedService<IPipelineProvider>(TProvider.Identifier);
      var rawString = await provider.RunRawString(parameters);
      
      return JsonSerializer.Deserialize<TResult>(rawString, _jsonOptions)
         ?? throw new InvalidOperationException("Invalid json string.");
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
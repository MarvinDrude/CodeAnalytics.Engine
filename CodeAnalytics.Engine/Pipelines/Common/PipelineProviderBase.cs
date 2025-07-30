using System.Text.Json;
using CodeAnalytics.Engine.Contracts.Pipelines.Interfaces;
using CodeAnalytics.Engine.Contracts.Pipelines.Models;
using CodeAnalytics.Engine.Json.Extensions;

namespace CodeAnalytics.Engine.Pipelines.Common;

public abstract class PipelineProviderBase
{
   private static readonly JsonSerializerOptions _jsonOptions = GetJsonSerializerOptions();

   protected string Serialize<T>(T ob)
   {
      return JsonSerializer.Serialize(ob, _jsonOptions);
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
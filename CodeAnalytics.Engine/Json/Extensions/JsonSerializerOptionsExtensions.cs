using System.Text.Json;
using CodeAnalytics.Engine.Json.Converters;

namespace CodeAnalytics.Engine.Json.Extensions;

public static class JsonSerializerOptionsExtensions
{
   public static void AddConverters(this JsonSerializerOptions options)
   {
      options.Converters.Add(new StringIdConverter());
      options.Converters.Add(new NodeIdConverter());
   }
}
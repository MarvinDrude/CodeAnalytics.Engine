using System.Text.Json;
using System.Text.Json.Serialization;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Ids;

namespace CodeAnalytics.Engine.Json.Converters;

public sealed class StringIdConverter : JsonConverter<StringId>
{
   public override StringId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
   {
      if (reader.TryGetInt32(out var value))
      {
         return new StringId(value, StringIdStore.Current);
      }
      
      throw new JsonException("Expected int value");
   }

   public override void Write(Utf8JsonWriter writer, StringId value, JsonSerializerOptions options)
   {
      writer.WriteNumberValue(value.Value);
   }
   
   public override StringId ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
   {
      var str = reader.GetString()
         ?? throw new JsonException("Expected string value");
      
      return new StringId(int.Parse(str), StringIdStore.Current);
   }

   public override void WriteAsPropertyName(Utf8JsonWriter writer, StringId value, JsonSerializerOptions options)
   {
      writer.WritePropertyName(value.Value.ToString());
   }
}
using System.Net.Http.Json;
using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Engine.Serialization;
using CodeAnalytics.Engine.Serialization.Occurrence;
using CodeAnalytics.Web.Common.Constants.Data;
using CodeAnalytics.Web.Common.Services.Data;

namespace CodeAnalytics.Web.Client.Services.Data;

public sealed class ClientOccurrenceService : IOccurrenceService
{
   private readonly HttpClient _client;

   public ClientOccurrenceService(HttpClient client)
   {
      _client = client;
   }

   public async Task<Dictionary<int, string>?> GetOccurrenceStrings(int rawNodeId)
   {
      var url = $"{DataApiConstants.FullPathGetOccurrenceStrings}?rawNodeId={rawNodeId}";
      var strings = await _client.GetFromJsonAsync<Dictionary<int, string>>(url);

      return strings;
   }
   
   public async Task<GlobalOccurrence?> GetOccurrences(int rawNodeId)
   {
      var url = $"{DataApiConstants.FullPathGetOccurrences}?rawNodeId={rawNodeId}";
      var bytes = await _client.GetByteArrayAsync(url);

      if (bytes.Length == 0)
      {
         return null;
      }

      return Serializer<GlobalOccurrence, GlobalOccurrenceSerializer>.FromMemory(bytes);
   }
}
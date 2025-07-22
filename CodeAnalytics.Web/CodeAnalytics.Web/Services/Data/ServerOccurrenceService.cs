using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Occurrences;
using CodeAnalytics.Web.Common.Services.Data;

namespace CodeAnalytics.Web.Services.Data;

public sealed class ServerOccurrenceService : IOccurrenceService
{
   private readonly IDataService _dataService;

   public ServerOccurrenceService(IDataService dataService)
   {
      _dataService = dataService;
   }
   
   public async Task<GlobalOccurrence?> GetOccurrences(int rawNodeId)
   {
      var store = await _dataService.GetAnalyzeStore();
      return store.Inner.Occurrences.Get(new NodeId(rawNodeId, null));
   }
}
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

   public async Task<Dictionary<int, string>?> GetOccurrenceStrings(int rawNodeId)
   {
      var store = await _dataService.GetAnalyzeStore();
      if (store.Inner.Occurrences.Get(new NodeId(rawNodeId, null)) is not { } occurrence)
      {
         return null;
      }

      HashSet<StringId> ids = [];
      foreach (var (key, project) in occurrence.ProjectOccurrences)
      {
         ids.Add(key);
         foreach (var (fileId, _) in project.FileOccurrences)
         {
            ids.Add(fileId);
         }
      }

      foreach (var (key, _) in occurrence.DeclarationMap)
      {
         ids.Add(key.FileId);
      }

      Dictionary<int, string> result = [];
      foreach (var id in ids)
      {
         if (store.Inner.StringIdStore.GetById(id) is { } str)
         {
            result[id] = str;
         }
      }

      return result;
   }
   
   public async Task<GlobalOccurrence?> GetOccurrences(int rawNodeId)
   {
      var store = await _dataService.GetAnalyzeStore();
      return store.Inner.Occurrences.Get(new NodeId(rawNodeId, null));
   }
}
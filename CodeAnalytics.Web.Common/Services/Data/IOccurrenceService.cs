using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Occurrences;

namespace CodeAnalytics.Web.Common.Services.Data;

public interface IOccurrenceService
{
   public Task<Dictionary<int, string>?> GetOccurrenceStrings(int rawNodeId);
   
   public Task<GlobalOccurrence?> GetOccurrences(int rawNodeId);
}
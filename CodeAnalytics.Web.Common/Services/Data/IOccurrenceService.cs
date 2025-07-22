using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Contracts.Occurrences;

namespace CodeAnalytics.Web.Common.Services.Data;

public interface IOccurrenceService
{
   public Task<GlobalOccurrence?> GetOccurrences(int rawNodeId);
}
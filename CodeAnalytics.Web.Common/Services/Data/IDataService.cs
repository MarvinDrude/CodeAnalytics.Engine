using CodeAnalytics.Engine.Analyze;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Web.Common.Services.Data;

public interface IDataService
{
   public Task<AnalyzeStore> GetAnalyzeStore();
}
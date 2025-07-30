using CodeAnalytics.Engine.Analyze;
using CodeAnalytics.Engine.Analyze.Interfaces;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Web.Common.Services.Data;

public interface IDataService : IAnalyzeStoreProvider
{
   public Task<AnalyzeStore> GetAnalyzeStore();
}
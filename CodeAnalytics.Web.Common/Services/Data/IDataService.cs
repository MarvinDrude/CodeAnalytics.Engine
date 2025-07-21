using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Web.Common.Services.Data;

public interface IDataService
{
   public Task<string> GetStringById(StringId id);
}
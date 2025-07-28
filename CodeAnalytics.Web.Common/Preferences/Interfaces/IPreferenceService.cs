using CodeAnalytics.Web.Common.Preferences.Models;

namespace CodeAnalytics.Web.Common.Preferences.Interfaces;

public interface IPreferenceService
{
   public int GetLatestVersion<TPreference>()
      where TPreference : IPreference;
   
   public Task<TPreference> GetOrCreate<TPreference>(Func<Task<TPreference>> createFactory) 
      where TPreference : IPreference;

   public Task Update<TPreference>(TPreference preference)
      where TPreference : IPreference;
}
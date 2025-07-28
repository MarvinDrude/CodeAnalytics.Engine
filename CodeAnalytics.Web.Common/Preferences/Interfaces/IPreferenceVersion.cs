using CodeAnalytics.Web.Common.Preferences.Models;

namespace CodeAnalytics.Web.Common.Preferences.Interfaces;

public interface IPreferenceVersion
{
   public int GetLatestVersion<TPreference>()
      where TPreference : IPreference;
}
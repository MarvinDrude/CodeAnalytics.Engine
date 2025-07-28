using CodeAnalytics.Web.Common.Preferences.Interfaces;
using CodeAnalytics.Web.Common.Preferences.Models;
using CodeAnalytics.Web.Common.Preferences.Search;

namespace CodeAnalytics.Web.Common.Preferences.Services;

public sealed class PreferenceVersion : IPreferenceVersion
{
   public int GetLatestVersion<TPreference>() 
      where TPreference : IPreference
   {
      return _versions.GetValueOrDefault(typeof(TPreference), -1);
   }
   
   private readonly Dictionary<Type, int> _versions = new ()
   {
      [typeof(DefinitionSearchPreference)] = 1,
      [typeof(FileSearchPreference)] = 1,
   };
}
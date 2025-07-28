using CodeAnalytics.Web.Common.Preferences.Models;

namespace CodeAnalytics.Web.Common.Preferences.Search;

public sealed class DefinitionSearchPreference 
   : Preference<DefinitionSearchPreferenceData>
{
   
}

public sealed class DefinitionSearchPreferenceData
{
   public string? SearchText { get; set; }

   public HashSet<string> Types { get; set; } = [];
   
   public int ScrollTop { get; set; } = 0;
}
using CodeAnalytics.Web.Common.Enums.Explorer;
using CodeAnalytics.Web.Common.Preferences.Models;

namespace CodeAnalytics.Web.Common.Preferences.Search;

public sealed class FileSearchPreference
   : Preference<FileSearchPreferenceData>
{
   
}

public sealed class FileSearchPreferenceData
{
   public string? SearchText { get; set; }

   public HashSet<ExplorerTreeItemType> Types { get; set; } = [];
}
using CodeAnalytics.Engine.Contracts.Analyze.Searchers;

namespace CodeAnalytics.Web.Common.Models.Search;

public sealed class BasicSearchParameters
{
   public required BaseSearchOptions Options { get; set; }
}
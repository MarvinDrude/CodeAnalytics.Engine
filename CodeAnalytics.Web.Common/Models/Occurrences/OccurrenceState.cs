using CodeAnalytics.Engine.Contracts.Occurrences;

namespace CodeAnalytics.Web.Common.Models.Occurrences;

public sealed class OccurrenceState
{
   public required GlobalOccurrence Occurrence { get; set; }
   
   public required Dictionary<int, string> Strings { get; set; }
   
   public bool ShouldNavigateToDefinition { get; set; }
}
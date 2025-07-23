using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;

namespace CodeAnalytics.Web.Common.Responses.Search;

public sealed class BasicSearchResponse
{
   public required List<IArchetype> Results { get; set; }
   
   public required int MaxResults { get; set; }
   
   public required Dictionary<int, string> Strings { get; set; } 
}
namespace Beskar.CodeAnalytics.Data.Indexes.Search;

public sealed class NGramSearchQuery
{
   public required string Text { get; set; }

   public NGramSearchQueryType QueryType { get; set; } = NGramSearchQueryType.Contains;

   public long Limit { get; set; } = long.MaxValue;
}

public enum NGramSearchQueryType
{
   StartsWith = 1,
   EndsWith = 2,
   Contains = 3
}
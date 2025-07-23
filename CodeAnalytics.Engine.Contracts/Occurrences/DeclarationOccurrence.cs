using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Occurrences;

public sealed record DeclarationOccurrence
{
   public NodeId NodeId { get; init; }
   public StringId FileId { get; init; }
   
   public int LineNumber { get; init; }
   public int SpanIndex { get; init; }
}
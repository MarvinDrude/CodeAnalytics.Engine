namespace CodeAnalytics.Web.Common.Models.Canvas.Graphs;

public sealed record ValueGraphNode
{
   public required string Name { get; init; }
   public required string Color { get; init; }
   
   public required double Value { get; init; }
}
using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.TextRendering;

namespace CodeAnalytics.Engine.Contracts.Occurrences;

public sealed class NodeOccurrence 
   : IEquatable<NodeOccurrence>
{
   public List<SyntaxSpan> LineSpans { get; init; } = [];
   public int SpanIndex { get; init; }
   
   public bool Equals(NodeOccurrence? other)
   {
      return other is not null && SpanIndex == other.SpanIndex;
   }

   public override bool Equals(object? obj)
   {
      return obj is NodeOccurrence other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(SpanIndex);
   }

   public static bool operator ==(NodeOccurrence left, NodeOccurrence right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(NodeOccurrence left, NodeOccurrence right)
   {
      return !(left == right);
   }
}
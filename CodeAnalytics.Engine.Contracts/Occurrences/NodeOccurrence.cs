using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.TextRendering;

namespace CodeAnalytics.Engine.Contracts.Occurrences;

public sealed class NodeOccurrence 
   : IEquatable<NodeOccurrence>
{
   public List<SyntaxSpan> LineSpans { get; init; } = [];
   public int SpanIndex { get; init; }

   private PackedBools _packed;
   public byte Flags
   {
      get => _packed.RawByte;
      set => _packed = new PackedBools(value);
   }

   public bool IsDeclaration
   {
      get => _packed.Get(IsDeclarationIndex);
      set => _packed.Set(IsDeclarationIndex, value);
   }

   private const int IsDeclarationIndex = 0;
   
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
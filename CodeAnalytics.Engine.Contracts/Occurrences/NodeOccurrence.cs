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

   private int _lineNumber = 0;

   public int LineNumber
   {
      get => _lineNumber;
      set
      {
         HasLineNumber = value > 0;
         
         switch (value)
         {
            case < byte.MaxValue:
               IsLineNumberByte = true;
               IsLineNumberUshort = false;
               break;
            case < ushort.MaxValue:
               IsLineNumberUshort = true;
               IsLineNumberByte = false;
               break;
            default:
               IsLineNumberByte = false;
               IsLineNumberUshort = false;
               break;
         }
         
         _lineNumber = Math.Max(value, 0);
      }
   }
   
   public bool IsLineNumberUshort
   {
      get => _packed.Get(LineNumberUshortIndex);
      private set => _packed.Set(LineNumberUshortIndex, value);
   }
   
   public bool IsLineNumberByte
   {
      get => _packed.Get(LineNumberByteIndex);
      private set => _packed.Set(LineNumberByteIndex, value);
   }

   public bool HasLineNumber
   {
      get => _packed.Get(HasLineNumberIndex);
      private set => _packed.Set(HasLineNumberIndex, value);
   }

   public const int LineNumberUshortIndex = 3;
   public const int LineNumberByteIndex = 2;
   public const int HasLineNumberIndex = 1;
   public const int IsDeclarationIndex = 0;
   
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
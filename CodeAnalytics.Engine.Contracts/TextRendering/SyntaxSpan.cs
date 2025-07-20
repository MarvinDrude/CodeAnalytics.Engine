using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.TextRendering;

[StructLayout(LayoutKind.Auto)]
public struct SyntaxSpan 
   : IEquatable<SyntaxSpan>
{
   public NodeId Reference = NodeId.Empty;
   
   public readonly string RawText;
   public readonly string Color;

   public readonly bool IsLineBreak;
   
   public SyntaxSpan(
      string rawText,
      string color,
      bool isLineBreak = false)
   {
      RawText = rawText;
      Color = color;
      
      IsLineBreak = isLineBreak;
   }

   public bool Equals(SyntaxSpan other)
   {
      return Reference.Equals(other.Reference) 
             && RawText == other.RawText 
             && Color == other.Color 
             && IsLineBreak == other.IsLineBreak;
   }

   public override bool Equals(object? obj)
   {
      return obj is SyntaxSpan other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Reference, RawText, Color, IsLineBreak);
   }

   public static bool operator ==(SyntaxSpan left, SyntaxSpan right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(SyntaxSpan left, SyntaxSpan right)
   {
      return !(left == right);
   }
}
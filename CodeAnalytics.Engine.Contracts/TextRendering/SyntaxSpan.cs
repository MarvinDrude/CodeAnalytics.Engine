using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.TextRendering;

[StructLayout(LayoutKind.Auto)]
public struct SyntaxSpan 
   : IEquatable<SyntaxSpan>
{
   public NodeId Reference = NodeId.Empty;
   public string StringReference = string.Empty;
   
   public readonly string RawText;
   public readonly string Color;

   public PackedBools Flags;
   
   public bool IsLineBreak
   {
      get => Flags.Get(IsLineBreakIndex);
      set => Flags.Set(IsLineBreakIndex, value);
   }

   public bool IsDeclaration
   {
      get => Flags.Get(IsDeclarationIndex);
      set => Flags.Set(IsDeclarationIndex, value);
   }
   
   public SyntaxSpan(
      string rawText,
      string color,
      bool isLineBreak = false)
   {
      RawText = rawText;
      Color = color;
      
      Flags = new PackedBools();
      IsLineBreak = isLineBreak;
   }

   private const int IsLineBreakIndex = 0;
   private const int IsDeclarationIndex = 1;

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
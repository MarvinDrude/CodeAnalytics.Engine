using System.Runtime.InteropServices;

namespace CodeAnalytics.Web.Common.Models.Canvas.Common;

[StructLayout(LayoutKind.Auto)]
public readonly struct BoundingRect : IEquatable<BoundingRect>
{
   public readonly float Left;
   public readonly float Right;
   public readonly float Top;
   public readonly float Bottom;

   public BoundingRect(
      float top, float right, 
      float bottom, float left)
   {
      Left = left;
      Right = right;
      Top = top;
      Bottom = bottom;
   }

   public bool Equals(BoundingRect other)
   {
      return Left.Equals(other.Left) && Right.Equals(other.Right) && Top.Equals(other.Top) && Bottom.Equals(other.Bottom);
   }

   public override bool Equals(object? obj)
   {
      return obj is BoundingRect other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(Left, Right, Top, Bottom);
   }

   public static bool operator ==(BoundingRect left, BoundingRect right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(BoundingRect left, BoundingRect right)
   {
      return !(left == right);
   }
}
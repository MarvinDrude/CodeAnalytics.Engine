using System.Runtime.InteropServices;

namespace CodeAnalytics.Web.Common.Models.Canvas.Common;

[StructLayout(LayoutKind.Auto)]
public readonly struct Rect : IEquatable<Rect>
{
   public static readonly Rect Empty = new(0, 0, 0, 0);
   
   public readonly float X;
   public readonly float Y;
   public readonly float Width;
   public readonly float Height;

   public Rect(
      float x,
      float y,
      float width,
      float height)
   {
      X = x;
      Y = y;
      
      Width = width;
      Height = height;
   }

   public bool Equals(Rect other)
   {
      return X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);
   }

   public override bool Equals(object? obj)
   {
      return obj is Rect other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(X, Y, Width, Height);
   }

   public static bool operator ==(Rect left, Rect right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(Rect left, Rect right)
   {
      return !(left == right);
   }
}
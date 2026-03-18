using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.Data.Entities.Misc;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[DebuggerDisplay("Offset: {Offset,nq}")]
public readonly struct StringFileView(ulong offset) 
   : IComparable<StringFileView>, IEquatable<StringFileView>
{
   public static readonly StringFileView Empty = new (0);
   
   public readonly ulong Offset = offset;

   public int CompareTo(StringFileView other)
   {
      return Offset.CompareTo(other.Offset);
   }

   public bool Equals(StringFileView other)
   {
      return Offset == other.Offset;
   }

   public override bool Equals(object? obj)
   {
      return obj is StringFileView other && Equals(other);
   }

   public override int GetHashCode()
   {
      return Offset.GetHashCode();
   }

    public static bool operator ==(StringFileView left, StringFileView right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(StringFileView left, StringFileView right)
    {
        return !(left == right);
    }
}
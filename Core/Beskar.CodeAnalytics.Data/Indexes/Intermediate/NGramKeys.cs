using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Beskar.CodeAnalytics.Data.Indexes.Intermediate;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[DebuggerDisplay("{MaterializedString,nq}")]
public struct NGram4
{
   public byte Length;
   public unsafe fixed byte Bytes[16];
   
   public unsafe string MaterializedString
   {
      get
      {
         fixed (byte* ptr = Bytes)
         {
            var span = new ReadOnlySpan<byte>(ptr, Length);
            return Encoding.UTF8.GetString(span);
         }
      }
   }
   
   public unsafe ReadOnlySpan<byte> AsSpan()
   {
      fixed (byte* ptr = Bytes)
      {
         return new ReadOnlySpan<byte>(ptr, Length);
      }
   }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[DebuggerDisplay("{MaterializedString,nq}")]
public struct NGram3
{
   public byte Length;
   public unsafe fixed byte Bytes[12];

   public unsafe string MaterializedString
   {
      get
      {
         fixed (byte* ptr = Bytes)
         {
            var span = new ReadOnlySpan<byte>(ptr, Length);
            return Encoding.UTF8.GetString(span);
         }
      }
   }
   
   public unsafe ReadOnlySpan<byte> AsSpan()
   {
      fixed (byte* ptr = Bytes)
      {
         return new ReadOnlySpan<byte>(ptr, Length);
      }
   }
}

public static class NGramEquality
{
   public static unsafe bool EqualsFast<T>(ref T left, ref T right)
      where T : unmanaged
   {
      var size = sizeof(T);

      fixed (T* pLeft = &left)
      fixed (T* pRight = &right)
      {
         var spanLeft = new ReadOnlySpan<byte>(pLeft, size);
         var spanRight = new ReadOnlySpan<byte>(pRight, size);
         
         return spanLeft.SequenceEqual(spanRight);
      }
   }

   public static unsafe int CompareFast<T>(ref T x, ref T y, int length)
      where T : unmanaged
   {
      fixed (void* ptrX = &x)
      fixed (void* ptrY = &y)
      {
         var spanX = new ReadOnlySpan<byte>((byte*)ptrX + 1, length);
         var spanY = new ReadOnlySpan<byte>((byte*)ptrY + 1, length);

         return spanX.SequenceCompareTo(spanY);
      }
   }

   public static unsafe int CompareIgnoreCase<T>(ref NGram3 x, ref NGram3 y)
   {
      Span<char> charsX = stackalloc char[x.Length];
      Span<char> charsY = stackalloc char[y.Length];

      Encoding.UTF8.GetChars(x.AsSpan(), charsX);
      Encoding.UTF8.GetChars(y.AsSpan(), charsY);

      return charsX.CompareTo(charsY, StringComparison.OrdinalIgnoreCase);
   }
}
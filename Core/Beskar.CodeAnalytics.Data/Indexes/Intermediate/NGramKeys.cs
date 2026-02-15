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
}
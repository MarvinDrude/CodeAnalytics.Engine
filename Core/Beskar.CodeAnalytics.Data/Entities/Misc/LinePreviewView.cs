using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.Data.Entities.Misc;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct LinePreviewView(ulong offset, int length)
{
   public ulong Offset = offset;
   public int Length = length;

   public int TokenStart;
   public int TokenLength;
}
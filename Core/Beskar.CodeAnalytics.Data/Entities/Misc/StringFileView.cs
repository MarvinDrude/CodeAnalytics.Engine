using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.Data.Entities.Misc;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct StringFileView(ulong offset)
{
   public static readonly StringFileView Empty = new (0);
   
   public readonly ulong Offset = offset;
}
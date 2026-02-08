using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.FileStorage.Models.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct DataPageFooter
{
   public uint Checksum;
}
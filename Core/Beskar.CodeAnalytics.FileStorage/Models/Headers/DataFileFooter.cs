using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.FileStorage.Models.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct DataFileFooter
{
   public uint MagicNumber;
   public long Timestamp;
}
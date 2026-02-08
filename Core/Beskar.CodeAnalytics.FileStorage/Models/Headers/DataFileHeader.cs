using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.FileStorage.Models.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct DataFileHeader
{
   public uint MagicNumber;
   public uint Version;
   public Guid UniqueId;

   public int PageSize;
   public ulong PageCount;

   public ulong ItemCount;
}
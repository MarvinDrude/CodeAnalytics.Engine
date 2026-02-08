using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.FileStorage.Models.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct DataPageHeader
{
   public ulong MinKey;
   public ulong MaxKey;
   
   public int ItemCount;
}
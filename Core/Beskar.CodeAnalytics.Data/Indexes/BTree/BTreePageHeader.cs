using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.Data.Indexes.BTree;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct BTreePageHeader
{
   public BTreePageType Type;
   public int ItemCount;
   
   public long NextPageOffset;
}
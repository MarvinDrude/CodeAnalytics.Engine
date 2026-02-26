using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.Data.Indexes.BTree;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct BTreeEntry<TKey>
   where TKey : unmanaged
{
   public TKey Key;
   public long PageOffset;
}
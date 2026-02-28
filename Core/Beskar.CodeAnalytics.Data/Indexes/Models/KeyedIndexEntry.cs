using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.Data.Indexes.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct KeyedIndexEntry<TKey>
   where TKey : unmanaged
{
   public TKey Key;
   public uint Id;
}
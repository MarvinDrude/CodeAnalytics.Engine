using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.Data.Indexes.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct DictionaryEntry<TKey>
   where TKey : unmanaged
{
   public TKey Key;
   
   public ulong Offset;
   public uint Count;
}
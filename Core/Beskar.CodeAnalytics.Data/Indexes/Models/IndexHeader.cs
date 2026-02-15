using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Enums.Indexes;

namespace Beskar.CodeAnalytics.Data.Indexes.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct IndexHeader
{
   public IndexType Type;
   
   public uint KeySize;
   public uint EntrySize;
   
   public ulong DictionaryOffset;
   public ulong DataOffset;
}
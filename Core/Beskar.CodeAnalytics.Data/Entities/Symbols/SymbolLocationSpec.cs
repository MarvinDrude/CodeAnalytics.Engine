using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SymbolLocationSpec : ISpec
{
   public uint SourceFileId;
   public uint SymbolId;

   public Flags8 Flags;
   
   public uint Identifier => SymbolId;
   public static FileId FileId => FileIds.FileLocation;

   public bool IsDefinition
   {
      get => Flags[0].Get(0);
      set => Flags[0].Set(0, value);
   }
}
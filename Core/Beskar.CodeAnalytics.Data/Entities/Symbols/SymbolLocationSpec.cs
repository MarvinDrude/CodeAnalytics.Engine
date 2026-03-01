using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SymbolLocationSpec : ISpec
{
   public uint SymbolId;
   public uint SourceFileId;

   public int LineNumber;
   public LinePreviewView LinePreview;
   
   public Flags8 Flags;
   
   public uint Identifier => SymbolId;
   public static FileId FileId => FileIds.FileLocation;

   public bool IsDeclaration
   {
      get => Flags[0].Get(0);
      set => Flags[0].Set(0, value);
   }
   
   public static readonly IComparer<SymbolLocationSpec> Comparer = Comparer<SymbolLocationSpec>.Create(
      static (x, y) =>
      {
         var compareSymbol = x.SymbolId.CompareTo(y.SymbolId);
         if (compareSymbol != 0) return compareSymbol;
         
         var isDeclaration = x.IsDeclaration.CompareTo(y.IsDeclaration);
         if (isDeclaration != 0) return isDeclaration;
         
         return x.SourceFileId.CompareTo(y.SourceFileId);
      });
}
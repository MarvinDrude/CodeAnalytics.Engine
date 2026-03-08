using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Structure;

namespace Beskar.CodeAnalytics.Data.Entities.Structure;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FileSpec : ISpec
{
   public uint Id;

   public FileKind Kind;
   
   public StringFileView FullPath;
   public StringFileView Name;
   
   public StorageView<SymbolSpec> Symbols;
   public StorageView<SymbolSpec> Declarations;
   
   public uint Identifier => Id;
   public static FileId FileId => FileIds.File;
}
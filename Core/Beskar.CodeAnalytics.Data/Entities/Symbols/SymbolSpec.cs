using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SymbolSpec : ISpec
{
   public uint Id;
   public uint ContainingId;
   public uint SourceFileId;
   
   public SymbolType Type;
   public AccessModifier Accessibility;
   public PackedBools8 Flags;

   public StringFileView Name;
   public StringFileView MetadataName;
   public StringFileView FullPathName;

   public StorageView<SymbolLocationSpec> Declarations;
   public StorageView<SymbolLocationSpec> Locations;
   
   public uint Identifier => Id;
   public static FileId FileId => FileIds.Symbol;

   public bool IsAbstract
   {
      get => Flags.Get(0);
      set => Flags.Set(0, value);
   }

   public bool HasContaining
   {
      get => Flags.Get(1);
      set => Flags.Set(1, value);
   }

   public bool IsExtern
   {
      get => Flags.Get(2);
      set => Flags.Set(2, value);
   }
   
   public bool IsImplicitlyDeclared
   {
      get => Flags.Get(3);
      set => Flags.Set(3, value);
   }
   
   public bool IsOverride 
   {
      get => Flags.Get(4);
      set => Flags.Set(4, value);
   }

   public bool IsSealed
   {
      get => Flags.Get(5);
      set => Flags.Set(5, value);
   }
   
   public bool IsStatic
   {
      get => Flags.Get(6);
      set => Flags.Set(6, value);
   }
   
   public bool IsVirtual
   {
      get => Flags.Get(7);
      set => Flags.Set(7, value);
   }
}
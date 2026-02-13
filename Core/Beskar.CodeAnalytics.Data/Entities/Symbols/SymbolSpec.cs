using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SymbolSpec
{
   public uint Id;
   public uint ContainingId;
   
   public SymbolType Type;
   public AccessModifier Accessibility;
   public Flags8 Flags;

   public StringFileView Name;
   public StringFileView MetadataName;
   public StringFileView FullPathName;

   public StorageView<SymbolLocationSpec> Declarations;
   public StorageView<SymbolLocationSpec> Locations;

   public bool IsAbstract
   {
      get => Flags[0].Get(0);
      set => Flags[0].Set(0, value);
   }

   public bool HasContaining
   {
      get => Flags[0].Get(1);
      set => Flags[0].Set(1, value);
   }

   public bool IsExtern
   {
      get => Flags[0].Get(2);
      set => Flags[0].Set(2, value);
   }
   
   public bool IsImplicitlyDeclared
   {
      get => Flags[0].Get(3);
      set => Flags[0].Set(3, value);
   }
   
   public bool IsOverride 
   {
      get => Flags[0].Get(4);
      set => Flags[0].Set(4, value);
   }

   public bool IsSealed
   {
      get => Flags[0].Get(5);
      set => Flags[0].Set(5, value);
   }
   
   public bool IsStatic
   {
      get => Flags[0].Get(6);
      set => Flags[0].Set(6, value);
   }
   
   public bool IsVirtual
   {
      get => Flags[0].Get(7);
      set => Flags[0].Set(7, value);
   }
}
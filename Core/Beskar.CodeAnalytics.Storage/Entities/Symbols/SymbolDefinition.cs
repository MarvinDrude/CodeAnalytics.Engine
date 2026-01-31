using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;
using Beskar.CodeAnalytics.Storage.Entities.Misc;
using Beskar.CodeAnalytics.Storage.Enums.Symbols;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Storage.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct SymbolDefinition
{
   public ulong Id;
   public ulong ContainingId;

   public SymbolType Type;
   public AccessModifier Accessibility;
   public PackedBools Flags;

   public StringDefinition Name;
   public StringDefinition MetadataName;
   public StringDefinition FullPathName;
   
   public StorageSlice<SymbolLocationDefinition> Declarations;
   
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
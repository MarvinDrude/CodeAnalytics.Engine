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
   public PackedBools FlagsLow;

   public StringDefinition Name;
   public StringDefinition MetadataName;
   public StringDefinition FullPathName;
   public StringDefinition FullPathUniqueId;
   
   public StorageSlice<SymbolLocationDefinition> Declarations;
   
   public bool IsAbstract
   {
      get => FlagsLow.Get(0);
      set => FlagsLow.Set(0, value);
   }

   public bool HasContaining
   {
      get => FlagsLow.Get(1);
      set => FlagsLow.Set(1, value);
   }

   public bool IsExtern
   {
      get => FlagsLow.Get(2);
      set => FlagsLow.Set(2, value);
   }

   public bool IsImplicitlyDeclared
   {
      get => FlagsLow.Get(3);
      set => FlagsLow.Set(3, value);
   }

   public bool IsOverride
   {
      get => FlagsLow.Get(4);
      set => FlagsLow.Set(4, value);
   }

   public bool IsSealed
   {
      get => FlagsLow.Get(5);
      set => FlagsLow.Set(5, value);
   }

   public bool IsStatic
   {
      get => FlagsLow.Get(6);
      set => FlagsLow.Set(6, value);
   }

   public bool IsVirtual
   {
      get => FlagsLow.Get(7);
      set => FlagsLow.Set(7, value);
   }
}
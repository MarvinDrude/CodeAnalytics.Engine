using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;
using Beskar.CodeAnalytics.Storage.Entities.Misc;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Storage.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct NamedTypeSymbolDefinition
{
   public ulong SymbolId;
   public ulong EnumUnderlyingTypeId;

   public PackedBools FlagsLow;
   
   public StorageSlice<TypeParameterSymbolDefinition> TypeParameters;
   
   public StorageSlice<MethodSymbolDefinition> Methods;
   public StorageSlice<MethodSymbolDefinition> InstanceConstructors;
   public StorageSlice<MethodSymbolDefinition> StaticConstructors;
   
   public bool IsFileLocal
   {
      get => FlagsLow.Get(0);
      set => FlagsLow.Set(0, value);
   }
   
   public bool IsEnum
   {
      get => FlagsLow.Get(1);
      set => FlagsLow.Set(1, value);
   }
}
using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NamedTypeSymbolSpec : ISpec
{
   public uint SymbolId;
   public uint EnumUnderlyingTypeId;

   public Flags8 Flags;
   
   public StorageView<TypeParameterSymbolSpec> TypeParameters;
   
   public StorageView<MethodSymbolSpec> Methods;
   public StorageView<MethodSymbolSpec> InstanceConstructors;
   public StorageView<MethodSymbolSpec> StaticConstructors;
   
   public uint Identifier => SymbolId;
   
   public bool IsFileLocal
   {
      get => Flags[0].Get(0);
      set => Flags[0].Set(0, value);
   }
   
   public bool IsEnum
   {
      get => Flags[0].Get(1);
      set => Flags[0].Set(1, value);
   }
}
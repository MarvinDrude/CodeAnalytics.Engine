using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Symbols;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MethodSymbolSpec : ISpec
{
   public uint SymbolId;
   public uint ReturnTypeId;
   public uint OverriddenMethodId;

   public Flags8 Flags;
   public MethodType MethodType;
   
   public StorageView<ParameterSymbolSpec> Parameters;
   public StorageView<TypeParameterSymbolSpec> TypeParameters;
   
   public uint Identifier => SymbolId;
   public static FileId FileId => FileIds.MethodSymbol;
   
   public bool HasVoidReturn
   {
      get => Flags[0].Get(0);
      set => Flags[0].Set(0, value);
   }
   
   public bool ReturnsByRefReadonly
   {
      get => Flags[0].Get(2);
      set => Flags[0].Set(2, value);
   }
   
   public bool ReturnsByRef
   {
      get => Flags[0].Get(3);
      set => Flags[0].Set(3, value);
   }
   
   public bool HasOverriddenMethod
   {
      get => Flags[0].Get(4);
      set => Flags[0].Set(4, value);
   }
   
   public bool IsReadOnly
   {
      get => Flags[0].Get(5);
      set => Flags[0].Set(5, value);
   }
   
   public bool IsIterator
   {
      get => Flags[0].Get(6);
      set => Flags[0].Set(6, value);
   }
   
   public bool IsAsync
   {
      get => Flags[0].Get(7);
      set => Flags[0].Set(7, value);
   }
}
using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MethodSymbolSpec : ISpec
{
   public uint SymbolId;
   public uint ReturnTypeId;
   public uint OverriddenMethodId;

   public PackedBools8 Flags;
   public MethodType MethodType;
   
   public StorageView<ParameterSymbolSpec> Parameters;
   public StorageView<TypeParameterSymbolSpec> TypeParameters;
   
   public uint Identifier => SymbolId;
   public static FileId FileId => FileIds.MethodSymbol;
   
   public bool HasVoidReturn
   {
      get => Flags.Get(0);
      set => Flags.Set(0, value);
   }
   
   public bool ReturnsByRefReadonly
   {
      get => Flags.Get(2);
      set => Flags.Set(2, value);
   }
   
   public bool ReturnsByRef
   {
      get => Flags.Get(3);
      set => Flags.Set(3, value);
   }
   
   public bool HasOverriddenMethod
   {
      get => Flags.Get(4);
      set => Flags.Set(4, value);
   }
   
   public bool IsReadOnly
   {
      get => Flags.Get(5);
      set => Flags.Set(5, value);
   }
   
   public bool IsIterator
   {
      get => Flags.Get(6);
      set => Flags.Set(6, value);
   }
   
   public bool IsAsync
   {
      get => Flags.Get(7);
      set => Flags.Set(7, value);
   }
}
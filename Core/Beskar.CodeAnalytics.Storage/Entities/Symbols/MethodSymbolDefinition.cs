using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;
using Beskar.CodeAnalytics.Storage.Entities.Misc;
using Beskar.CodeAnalytics.Storage.Enums.Symbols;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Storage.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct MethodSymbolDefinition
{
   public ulong SymbolId;
   public ulong ReturnTypeId;
   public ulong OverriddenMethodId;

   public PackedBools FlagsLow;
   public MethodType MethodType;
   
   public StorageSlice<ParameterSymbolDefinition> Parameters;
   public StorageSlice<TypeParameterSymbolDefinition> TypeParameters;
   
   public bool HasVoidReturn
   {
      get => FlagsLow.Get(0);
      set => FlagsLow.Set(0, value);
   }
   
   public bool ReturnsByRefReadonly
   {
      get => FlagsLow.Get(2);
      set => FlagsLow.Set(2, value);
   }
   
   public bool ReturnsByRef
   {
      get => FlagsLow.Get(3);
      set => FlagsLow.Set(3, value);
   }
   
   public bool HasOverriddenMethod
   {
      get => FlagsLow.Get(4);
      set => FlagsLow.Set(4, value);
   }
   
   public bool IsReadOnly
   {
      get => FlagsLow.Get(5);
      set => FlagsLow.Set(5, value);
   }
   
   public bool IsIterator
   {
      get => FlagsLow.Get(6);
      set => FlagsLow.Set(6, value);
   }
   
   public bool IsAsync
   {
      get => FlagsLow.Get(7);
      set => FlagsLow.Set(7, value);
   }
}
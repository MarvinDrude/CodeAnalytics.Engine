using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;
using Beskar.CodeAnalytics.Storage.Enums.Symbols;
using Beskar.CodeAnalytics.Storage.Interfaces;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Storage.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct ParameterSymbolDefinition : ISecondarySymbolDefinition
{
   public ulong OriginId => SymbolId;
   
   public ulong SymbolId;
   public ulong TypeId;

   public int Ordinal;
   public ScopeType ScopeType;
   public RefType RefType;
   
   public PackedBools FlagsLow;
   
   public bool HasExplicitDefaultValue
   {
      get => FlagsLow.Get(0);
      set => FlagsLow.Set(0, value);
   }
   
   public bool IsParamsArray
   {
      get => FlagsLow.Get(1);
      set => FlagsLow.Set(1, value);
   }
   
   public bool IsParamsCollection
   {
      get => FlagsLow.Get(2);
      set => FlagsLow.Set(2, value);
   }
   
   public bool IsDiscard
   {
      get => FlagsLow.Get(3);
      set => FlagsLow.Set(3, value);
   }
   
   public bool IsOptional
   {
      get => FlagsLow.Get(4);
      set => FlagsLow.Set(4, value);
   }
}
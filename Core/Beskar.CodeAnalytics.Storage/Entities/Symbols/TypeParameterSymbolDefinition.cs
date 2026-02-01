using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;
using Beskar.CodeAnalytics.Storage.Entities.Misc;
using Beskar.CodeAnalytics.Storage.Interfaces;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Storage.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct TypeParameterSymbolDefinition : ISecondarySymbolDefinition
{
   public ulong OriginId => SymbolId;
   
   public ulong SymbolId;

   public int Ordinal;
   public PackedBools FlagsLow;

   public StorageSlice<TypeSymbolDefinition> ConstraintTypes;
   
   public bool AllowsRefLikeType
   {
      get => FlagsLow.Get(0);
      set => FlagsLow.Set(0, value);
   }
   
   public bool HasConstructorConstraint
   {
      get => FlagsLow.Get(1);
      set => FlagsLow.Set(1, value);
   }
   
   public bool HasNotNullConstraint
   {
      get => FlagsLow.Get(2);
      set => FlagsLow.Set(2, value);
   }
   
   public bool HasReferenceTypeConstraint
   {
      get => FlagsLow.Get(3);
      set => FlagsLow.Set(3, value);
   }
   
   public bool HasUnmanagedTypeConstraint
   {
      get => FlagsLow.Get(4);
      set => FlagsLow.Set(4, value);
   }
   
   public bool HasValueTypeConstraint
   {
      get => FlagsLow.Get(5);
      set => FlagsLow.Set(5, value);
   }
}
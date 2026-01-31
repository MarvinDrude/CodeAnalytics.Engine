using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;
using Beskar.CodeAnalytics.Storage.Entities.Misc;
using Beskar.CodeAnalytics.Storage.Enums.Symbols;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Storage.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct TypeSymbolDefinition
{
   public ulong SymbolId;
   public ulong BaseTypeId;

   public SpecialTypeKind SpecialType;
   public PackedBools FlagsLow;
   
   public StorageSlice<TypeSymbolDefinition> AllInterfaces;
   
   public bool HasBaseType
   {
      get => FlagsLow.Get(0);
      set => FlagsLow.Set(0, value);
   }

   public bool IsReadOnly
   {
      get => FlagsLow.Get(1);
      set => FlagsLow.Set(1, value);
   }

   public bool IsRecord
   {
      get => FlagsLow.Get(2);
      set => FlagsLow.Set(2, value);
   }
   
   public bool IsReferenceType
   {
      get => FlagsLow.Get(3);
      set => FlagsLow.Set(3, value);
   }
   
   public bool IsRefLikeType
   {
      get => FlagsLow.Get(4);
      set => FlagsLow.Set(4, value);
   }
   
   public bool IsTupleType
   {
      get => FlagsLow.Get(5);
      set => FlagsLow.Set(5, value);
   }
   
   public bool IsUnmanagedType
   {
      get => FlagsLow.Get(6);
      set => FlagsLow.Set(6, value);
   }
   
   public bool IsValueType
   {
      get => FlagsLow.Get(7);
      set => FlagsLow.Set(7, value);
   }
}
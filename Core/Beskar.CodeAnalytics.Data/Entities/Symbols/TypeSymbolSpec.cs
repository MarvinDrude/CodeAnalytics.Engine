using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TypeSymbolSpec : ISpec
{
   public uint SymbolId;
   public uint BaseTypeId;
   
   public TypeStorageKind Kind;
   public SpecialTypeKind SpecialType;
   public PackedBools8 Flags;

   public StorageView<TypeSymbolSpec> AllInterfaces;
   public StorageView<TypeSymbolSpec> DirectInterfaces;

   public uint Identifier => SymbolId;
   public static FileId FileId => FileIds.TypeSymbol;

   public bool HasBaseType
   {
      get => Flags.Get(0);
      set => Flags.Set(0, value);
   }

   public bool IsReadOnly
   {
      get => Flags.Get(1);
      set => Flags.Set(1, value);
   }

   public bool IsRecord
   {
      get => Flags.Get(2);
      set => Flags.Set(2, value);
   }
   
   public bool IsReferenceType
   {
      get => Flags.Get(3);
      set => Flags.Set(3, value);
   }
   
   public bool IsRefLikeType
   {
      get => Flags.Get(4);
      set => Flags.Set(4, value);
   }
   
   public bool IsTupleType
   {
      get => Flags.Get(5);
      set => Flags.Set(5, value);
   }
   
   public bool IsUnmanagedType
   {
      get => Flags.Get(6);
      set => Flags.Set(6, value);
   }
   
   public bool IsValueType
   {
      get => Flags.Get(7);
      set => Flags.Set(7, value);
   }
}
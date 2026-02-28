using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Symbols;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TypeSymbolSpec : ISpec
{
   public uint SymbolId;
   public uint BaseTypeId;
   
   public TypeStorageKind Kind;
   public SpecialTypeKind SpecialType;
   public Flags8 Flags;

   public StorageView<TypeSymbolSpec> AllInterfaces;
   public StorageView<TypeSymbolSpec> DirectInterfaces;

   public uint Identifier => SymbolId;
   public static FileId FileId => FileIds.TypeSymbol;

   public bool HasBaseType
   {
      get => Flags[0].Get(0);
      set => Flags[0].Set(0, value);
   }

   public bool IsReadOnly
   {
      get => Flags[0].Get(1);
      set => Flags[0].Set(1, value);
   }

   public bool IsRecord
   {
      get => Flags[0].Get(2);
      set => Flags[0].Set(2, value);
   }
   
   public bool IsReferenceType
   {
      get => Flags[0].Get(3);
      set => Flags[0].Set(3, value);
   }
   
   public bool IsRefLikeType
   {
      get => Flags[0].Get(4);
      set => Flags[0].Set(4, value);
   }
   
   public bool IsTupleType
   {
      get => Flags[0].Get(5);
      set => Flags[0].Set(5, value);
   }
   
   public bool IsUnmanagedType
   {
      get => Flags[0].Get(6);
      set => Flags[0].Set(6, value);
   }
   
   public bool IsValueType
   {
      get => Flags[0].Get(7);
      set => Flags[0].Set(7, value);
   }
}
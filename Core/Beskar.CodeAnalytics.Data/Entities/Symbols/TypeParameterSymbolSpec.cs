using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TypeParameterSymbolSpec : ISpec
{
   public uint SymbolId;

   public int Ordinal;
   public PackedBools8 Flags;

   public StorageView<TypeSymbolSpec> ConstraintTypes;
   
   public uint Identifier => SymbolId;
   public static FileId FileId => FileIds.TypeParameterSymbol;
   
   public bool AllowsRefLikeType
   {
      get => Flags.Get(0);
      set => Flags.Set(0, value);
   }
   
   public bool HasConstructorConstraint
   {
      get => Flags.Get(1);
      set => Flags.Set(1, value);
   }
   
   public bool HasNotNullConstraint
   {
      get => Flags.Get(2);
      set => Flags.Set(2, value);
   }
   
   public bool HasReferenceTypeConstraint
   {
      get => Flags.Get(3);
      set => Flags.Set(3, value);
   }
   
   public bool HasUnmanagedTypeConstraint
   {
      get => Flags.Get(4);
      set => Flags.Set(4, value);
   }
   
   public bool HasValueTypeConstraint
   {
      get => Flags.Get(5);
      set => Flags.Set(5, value);
   }
}
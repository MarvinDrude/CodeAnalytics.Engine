using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Entities.Misc;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TypeParameterSymbolSpec
{
   public uint SymbolId;

   public int Ordinal;
   public Flags8 Flags;

   public StorageView<TypeSymbolSpec> ConstraintTypes;
   
   public bool AllowsRefLikeType
   {
      get => Flags[0].Get(0);
      set => Flags[0].Set(0, value);
   }
   
   public bool HasConstructorConstraint
   {
      get => Flags[0].Get(1);
      set => Flags[0].Set(1, value);
   }
   
   public bool HasNotNullConstraint
   {
      get => Flags[0].Get(2);
      set => Flags[0].Set(2, value);
   }
   
   public bool HasReferenceTypeConstraint
   {
      get => Flags[0].Get(3);
      set => Flags[0].Set(3, value);
   }
   
   public bool HasUnmanagedTypeConstraint
   {
      get => Flags[0].Get(4);
      set => Flags[0].Set(4, value);
   }
   
   public bool HasValueTypeConstraint
   {
      get => Flags[0].Get(5);
      set => Flags[0].Set(5, value);
   }
}
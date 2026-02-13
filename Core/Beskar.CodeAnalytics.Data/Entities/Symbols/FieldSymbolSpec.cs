using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Symbols;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FieldSymbolSpec
{
   public uint SymbolId;
   public uint TypeId;
   
   public RefType RefType;
   public Flags8 Flags;
   
   public bool HasConstantValue
   {
      get => Flags[0].Get(0);
      set => Flags[0].Set(0, value);
   }
   
   public bool IsConst
   {
      get => Flags[0].Get(1);
      set => Flags[0].Set(1, value);
   }
   
   public bool IsRequired
   {
      get => Flags[0].Get(2);
      set => Flags[0].Set(2, value);
   }
   
   public bool IsVolatile
   {
      get => Flags[0].Get(3);
      set => Flags[0].Set(3, value);
   }
   
   public bool IsReadOnly
   {
      get => Flags[0].Get(4);
      set => Flags[0].Set(4, value);
   }
}
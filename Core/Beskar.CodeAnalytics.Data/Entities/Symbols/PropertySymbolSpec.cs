using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Symbols;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct PropertySymbolSpec
{
   public uint SymbolId;
   public uint TypeId;
   public uint GetMethodId;
   public uint SetMethodId;
   
   public RefType RefType;
   public Flags8 Flags;
   
   public bool HasGetter
   {
      get => Flags[0].Get(0);
      set => Flags[0].Set(0, value);
   }
   
   public bool HasSetter
   {
      get => Flags[0].Get(1);
      set => Flags[0].Set(1, value);
   }
   
   public bool IsReadOnly
   {
      get => Flags[0].Get(2);
      set => Flags[0].Set(2, value);
   }
   
   public bool IsIndexer
   {
      get => Flags[0].Get(3);
      set => Flags[0].Set(3, value);
   }
   
   public bool IsRequired
   {
      get => Flags[0].Get(4);
      set => Flags[0].Set(4, value);
   }
}
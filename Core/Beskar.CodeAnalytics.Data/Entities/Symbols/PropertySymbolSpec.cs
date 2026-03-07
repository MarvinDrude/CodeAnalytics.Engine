using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct PropertySymbolSpec : ISpec
{
   public uint SymbolId;
   public uint TypeId;
   public uint GetMethodId;
   public uint SetMethodId;
   
   public RefType RefType;
   public PackedBools8 Flags;
   
   public uint Identifier => SymbolId;
   public static FileId FileId => FileIds.PropertySymbol;
   
   public bool HasGetter
   {
      get => Flags.Get(0);
      set => Flags.Set(0, value);
   }
   
   public bool HasSetter
   {
      get => Flags.Get(1);
      set => Flags.Set(1, value);
   }
   
   public bool IsReadOnly
   {
      get => Flags.Get(2);
      set => Flags.Set(2, value);
   }
   
   public bool IsIndexer
   {
      get => Flags.Get(3);
      set => Flags.Set(3, value);
   }
   
   public bool IsRequired
   {
      get => Flags.Get(4);
      set => Flags.Set(4, value);
   }
}
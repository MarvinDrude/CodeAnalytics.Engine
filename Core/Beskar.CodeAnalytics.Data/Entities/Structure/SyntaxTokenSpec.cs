using System.Diagnostics;
using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Syntax;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Data.Entities.Structure;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SyntaxTokenSpec
{
   public int Start;
   public int Length;

   public SyntaxColor Color;
   public PackedBools8 Flags;

   public uint SymbolId;

   public bool IsLineBreak
   {
      get => Flags.Get(0);
      set => Flags.Set(0, value);
   }
   
   public bool IsKeyword
   {
      get => Flags.Get(1);
      set => Flags.Set(1, value);
   }
   
   public bool HasSymbol
   {
      get => Flags.Get(2);
      set => Flags.Set(2, value);
   }
   
   public bool IsDeclaration
   {
      get => Flags.Get(3);
      set => Flags.Set(3, value);
   }

   public bool IsPlain
   {
      get => Flags.Get(4);
      set => Flags.Set(4, value);
   }
}
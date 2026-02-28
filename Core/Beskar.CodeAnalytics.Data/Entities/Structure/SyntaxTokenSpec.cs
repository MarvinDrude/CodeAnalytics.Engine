using System.Diagnostics;
using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Syntax;

namespace Beskar.CodeAnalytics.Data.Entities.Structure;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SyntaxTokenSpec
{
   public int Start;
   public int Length;

   public SyntaxColor Color;
   public Flags8 Flags;

   public uint SymbolId;

   public bool IsLineBreak
   {
      get => Flags[0].Get(0);
      set => Flags[0].Set(0, value);
   }
   
   public bool IsKeyword
   {
      get => Flags[0].Get(1);
      set => Flags[0].Set(1, value);
   }
   
   public bool HasSymbol
   {
      get => Flags[0].Get(2);
      set => Flags[0].Set(2, value);
   }
   
   public bool IsDeclaration
   {
      get => Flags[0].Get(3);
      set => Flags[0].Set(3, value);
   }

   public bool IsPlain
   {
      get => Flags[0].Get(4);
      set => Flags[0].Set(4, value);
   }
}
using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Entities.Symbols;

namespace Beskar.CodeAnalytics.Data.Entities.Archetypes;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FieldArchetype
{
   public SymbolSpec Symbol;
   public FieldSymbolSpec Field;
}
using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Entities.Symbols;

namespace Beskar.CodeAnalytics.Data.Entities.Archetypes;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NamedTypeArchetype
{
   public SymbolSpec Symbol;
   public TypeSymbolSpec Type;
   public NamedTypeSymbolSpec NamedType;
}
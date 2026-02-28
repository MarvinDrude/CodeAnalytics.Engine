using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Enums.Symbols;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SymbolEdgeSpec
{
   public uint SourceSymbolId;
   public uint TargetSymbolId;

   public SymbolEdgeType Type;
}

public readonly record struct SymbolEdgeKey(
   uint SourceSymbolId, uint TargetSymbolId, SymbolEdgeType Type);
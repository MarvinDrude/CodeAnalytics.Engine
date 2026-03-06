using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Enums.Symbols;

namespace Beskar.CodeAnalytics.Data.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SymbolEdgeSpec : ISpec
{
   public uint SourceSymbolId;
   public uint TargetSymbolId;

   public SymbolEdgeType Type;

   public uint Identifier => TargetSymbolId;
   public static FileId FileId => FileIds.EdgeSymbol;
}

public readonly record struct SymbolEdgeKey(
   uint SourceSymbolId, uint TargetSymbolId, SymbolEdgeType Type);
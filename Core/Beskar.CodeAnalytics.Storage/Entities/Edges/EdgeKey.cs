using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;

namespace Beskar.CodeAnalytics.Storage.Entities.Edges;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public readonly record struct EdgeKey(
   ulong SourceSymbolId,
   ulong TargetSymbolId,
   EdgeType EdgeType);
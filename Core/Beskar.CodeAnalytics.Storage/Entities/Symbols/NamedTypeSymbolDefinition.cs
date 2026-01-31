using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;
using Beskar.CodeAnalytics.Storage.Entities.Misc;

namespace Beskar.CodeAnalytics.Storage.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct NamedTypeSymbolDefinition
{
   public ulong SymbolId;
   
   
   
   public StorageSlice<TypeParameterSymbolDefinition> TypeParameters;
}
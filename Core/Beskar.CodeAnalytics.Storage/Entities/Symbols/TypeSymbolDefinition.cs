using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;
using Me.Memory.Buffers.Dynamic;

namespace Beskar.CodeAnalytics.Storage.Entities.Symbols;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct TypeSymbolDefinition
{
   public int SymbolId;
   public int BaseTypeId;

   public PackedBools Flags;
   
}
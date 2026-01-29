using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;

namespace Beskar.CodeAnalytics.Storage.Entities.Misc;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct StringDefinition(ulong offset)
{
   public static readonly StringDefinition Empty = new (0);
   
   public readonly ulong Offset = offset;
}
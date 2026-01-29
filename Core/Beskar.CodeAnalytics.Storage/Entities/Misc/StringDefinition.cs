using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;

namespace Beskar.CodeAnalytics.Storage.Entities.Misc;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct StringDefinition(long offset)
{
   public static readonly StringDefinition Empty = new (-1);
   
   public readonly long Offset = offset;
}
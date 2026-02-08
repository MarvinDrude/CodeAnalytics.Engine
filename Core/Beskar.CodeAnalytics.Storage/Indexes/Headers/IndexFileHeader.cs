using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;

namespace Beskar.CodeAnalytics.Storage.Indexes.Headers;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct IndexFileHeader
{
   public byte DimensionCount;
}
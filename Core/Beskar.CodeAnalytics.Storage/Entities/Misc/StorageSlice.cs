using System.Runtime.InteropServices;
using Beskar.CodeAnalytics.Storage.Constants;

namespace Beskar.CodeAnalytics.Storage.Entities.Misc;

[StructLayout(LayoutKind.Sequential, Pack = StorageConstants.StructPacking)]
public struct StorageSlice<T>(int offset, int count)
   where T : unmanaged
{
   public int Offset = offset;
   public int Count = count;
}
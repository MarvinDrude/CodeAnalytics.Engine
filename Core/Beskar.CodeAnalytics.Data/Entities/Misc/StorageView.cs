using System.Runtime.InteropServices;

namespace Beskar.CodeAnalytics.Data.Entities.Misc;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct StorageView<T>(int offset, int count)
   where T : unmanaged
{
   public int Offset = offset;
   public int Count = count;
}
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Serialization;

namespace CodeAnalytics.Engine.Serialization.System.Common;

public sealed class UnmanagedSerializer<T> : ISerializer<T>
   where T : unmanaged
{
   public static void Serialize(ref ByteWriter writer, ref T ob)
   {
      writer.WriteLittleEndian(ob);
   }

   public static bool TryDeserialize(ref ByteReader reader, out T ob)
   {
      ob = reader.ReadLittleEndian<T>();
      return true;
   }
}
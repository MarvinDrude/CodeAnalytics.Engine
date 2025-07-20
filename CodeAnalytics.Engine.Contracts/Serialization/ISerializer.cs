using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;

namespace CodeAnalytics.Engine.Contracts.Serialization;

public interface ISerializer<T>
   where T : allows ref struct
{
   public static abstract void Serialize(ref ByteWriter writer, ref T ob);

   public static abstract bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out T ob);
}
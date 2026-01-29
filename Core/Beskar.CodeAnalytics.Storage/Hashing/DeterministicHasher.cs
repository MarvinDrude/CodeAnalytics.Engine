using System.Text;
using System.IO.Hashing;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Storage.Hashing;

public static class DeterministicHasher
{
   public static ulong GetDeterministicId(scoped in ReadOnlySpan<char> fullPath, long seed = 1337L)
   {
      var requiredSize = Encoding.UTF8.GetByteCount(fullPath);
      using var owner = requiredSize <= 1024
         ? new SpanOwner<byte>(stackalloc byte[requiredSize])
         : new SpanOwner<byte>(requiredSize);

      Encoding.UTF8.GetBytes(fullPath, owner.Span);
      return XxHash64.HashToUInt64(owner.Span, seed);
   }
}
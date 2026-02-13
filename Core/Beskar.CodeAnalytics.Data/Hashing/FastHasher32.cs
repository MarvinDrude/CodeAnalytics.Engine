using System.IO.Hashing;
using System.Text;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Data.Hashing;

public static class FastHasher32
{
   public static uint GetDeterministicId(scoped in ReadOnlySpan<char> input, int seed = _defaultSeed)
   {
      var requiredSize = Encoding.UTF8.GetByteCount(input);
      using var owner = requiredSize <= 1024
         ? new SpanOwner<byte>(stackalloc byte[requiredSize])
         : new SpanOwner<byte>(requiredSize);
      
      Encoding.UTF8.GetBytes(input, owner.Span);
      return GetDeterministicId(owner.Span, seed);
   }
   
   public static uint GetDeterministicId(scoped in ReadOnlySpan<byte> bytes, int seed = _defaultSeed)
   {
      return XxHash32.HashToUInt32(bytes, seed);
   }
   
   private const int _defaultSeed = 1337;
}
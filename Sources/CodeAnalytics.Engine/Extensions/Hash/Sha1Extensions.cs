using System.Security.Cryptography;
using System.Text;

namespace CodeAnalytics.Engine.Extensions.Hash;

public static class Sha1Extensions
{
   extension(SHA1 sha)
   {
      public static string CreateHexString(
         string str, 
         Encoding? encoding = null,
         bool toLowerCase = false)
      {
         if (str.Length == 0) return string.Empty;
         encoding ??= Encoding.UTF8;

         var maxByteCount = encoding.GetMaxByteCount(str.Length);
         var buffer = maxByteCount <= 256
            ? stackalloc byte[maxByteCount]
            : new byte[maxByteCount];

         var byteCount = encoding.GetBytes(str, buffer);
         Span<byte> hash = stackalloc byte[20];

         if (!SHA1.TryHashData(buffer[..byteCount], hash, out _))
         {
            throw new CryptographicException("SHA1 hashing failed.");
         }
         
         return toLowerCase
            ? Convert.ToHexStringLower(hash)
            : Convert.ToHexString(hash);
      }
   }
}
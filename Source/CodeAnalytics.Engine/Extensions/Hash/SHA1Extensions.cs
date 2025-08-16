using System.Security.Cryptography;
using System.Text;

namespace CodeAnalytics.Engine.Extensions.Hash;

public static class SHA1Extensions
{
   extension(SHA1 sha)
   {
      public static string CreateHexString(
         string str, 
         Encoding? encoding = null,
         bool toLowerCase = false)
      {
         encoding ??= Encoding.UTF8;
         Func<ReadOnlySpan<byte>, string> toHexFunc = toLowerCase
            ? Convert.ToHexStringLower
            : Convert.ToHexString;

         return toHexFunc(
            SHA1.HashData(encoding.GetBytes(str))
         );
      }
   }
}
using System.Globalization;
using System.Numerics;

namespace CodeAnalytics.Engine.Utils;

public static class CultureUtils
{
   public static readonly CultureInfo DefaultCulture = CreateDefaultCulture();

   public static string ToNumberString<TNumber>(this TNumber number)
      where TNumber : INumber<TNumber>
   {
      return number.ToString("#,0.###", DefaultCulture);
   }
   
   private static CultureInfo CreateDefaultCulture()
   {
      var info = (CultureInfo)CultureInfo.InvariantCulture.Clone();
      info.NumberFormat.NumberGroupSeparator = ".";
      info.NumberFormat.NumberDecimalSeparator = ".";

      return info;
   }
}
using System.Globalization;
using System.Numerics;

namespace Beskar.CodeAnalytics.Data.Utils.Formatting;

public static class NumberStrings
{
   private static readonly NumberFormatInfo DotSeparatorFormat = new()
   {
      NumberGroupSeparator = ".",
      NumberDecimalSeparator = ",",
      NumberGroupSizes = [3]
   };
   
   extension<T>(T num)
      where T : INumber<T>
   {
      public string ToDotString() => num.ToString("N2", DotSeparatorFormat);
      
      public string ToDotString(int precision) => num.ToString($"N{precision}", DotSeparatorFormat);
   }
}
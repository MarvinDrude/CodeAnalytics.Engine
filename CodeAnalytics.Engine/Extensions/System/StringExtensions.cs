using CodeAnalytics.Engine.Common.Buffers;

namespace CodeAnalytics.Engine.Extensions.System;

public static class StringExtensions
{
   public static string EscapeHtml(this string str)
   {
      if (string.IsNullOrWhiteSpace(str)) return str;
      using var writer = new TextWriterSlim(stackalloc char[256]);

      foreach (var c in str)
      {
         writer.Write(c switch
         {
            '<' => "&lt;",
            '>' => "&gt;",
            '"' => "&quot;",
            '\'' => "&apos;",
            '&' => "&amp;",
            _ => c.ToString()
         });
      }
      
      return writer.WrittenSpan.ToString();
   }
}
namespace Beskar.CodeAnalytics.Data.Extensions;

public static class StringExtensions
{
   extension(scoped in ReadOnlySpan<char> str)
   {
      public ReadOnlySpan<char> GetBaseFileName()
      {
         var index = str.IndexOf('.');
         return index == -1 ? str : str[..index];
      }
   }
}
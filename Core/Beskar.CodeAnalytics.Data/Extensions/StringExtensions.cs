namespace Beskar.CodeAnalytics.Data.Extensions;

public static class StringExtensions
{
   extension(string str)
   {
      public string GetBaseFileName()
      {
         var index = str.LastIndexOf('.');
         return index == -1 ? str : str[..index];
      }
   }
}
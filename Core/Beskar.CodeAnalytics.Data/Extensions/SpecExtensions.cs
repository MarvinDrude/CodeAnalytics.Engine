using Beskar.CodeAnalytics.Data.Entities.Interfaces;

namespace Beskar.CodeAnalytics.Data.Extensions;

public static class SpecExtensions
{
   extension<TSpec>(scoped in Span<TSpec> span)
      where TSpec : ISpec
   {
      public int BinaryFindIndex(uint identifier)
      {
         var low = 0;
         var high = span.Length - 1;

         while (low <= high)
         {
            var mid = low + ((high - low) >> 1);
            var midId = span[mid].Identifier;

            if (midId == identifier) return mid;
            if (midId < identifier) low = mid + 1;
            else high = mid - 1;
         }

         return -1;
      }
   }
}
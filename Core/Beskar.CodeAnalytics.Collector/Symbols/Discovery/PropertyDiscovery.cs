using Beskar.CodeAnalytics.Collector.Projects.Models;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Symbols.Discovery;

public static class PropertyDiscovery
{
   public static async Task<bool> Discover(DiscoverContext context, ulong id)
   {
      if (context.Symbol is not IPropertySymbol propertySymbol)
      {
         return false;
      }

      return true;
   }
}
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Symbols.Discovery;

public static class FieldDiscovery
{
   public static async Task<bool> Discover(DiscoverContext context, ulong id)
   {
      if (context.Symbol is not IFieldSymbol fieldSymbol)
      {
         return false;
      }

      var batch = context.DiscoveryBatch;
      
      

      return true;
   }
}
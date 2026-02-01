using Beskar.CodeAnalytics.Collector.Extensions;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Storage.Entities.Symbols;
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
      
      var batch = context.DiscoveryBatch;

      batch.TryGetDeterministicId(propertySymbol.Type, out var typeId);
      batch.TryGetDeterministicId(propertySymbol.GetMethod, out var getMethodId);
      batch.TryGetDeterministicId(propertySymbol.SetMethod, out var setMethodId);

      var definition = new PropertySymbolDefinition()
      {
         SymbolId = id,
         TypeId = typeId,
         GetMethodId = getMethodId,
         SetMethodId = setMethodId,
         
         RefType = propertySymbol.RefKind.ToStorage(),
         IsReadOnly = propertySymbol.IsReadOnly,
         IsRequired = propertySymbol.IsRequired,
         HasGetter = propertySymbol.GetMethod is not null,
         HasSetter = propertySymbol.SetMethod is not null,
         IsIndexer = propertySymbol.IsIndexer,
      };
      
      await batch.PropertySymbolWriter.Write(id, definition);
      return true;
   }
}
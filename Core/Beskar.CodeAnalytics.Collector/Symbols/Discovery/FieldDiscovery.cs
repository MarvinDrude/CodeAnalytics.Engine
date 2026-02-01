using Beskar.CodeAnalytics.Collector.Extensions;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Storage.Entities.Symbols;
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
      
      ulong typeId = 0;
      if (UniqueIdentifier.Create(fieldSymbol.Type) is { } typePath)
      {
         var stringDefinition = batch.StringDefinitions.GetStringDefinition(typePath);
         typeId = batch.Identifiers.GetDeterministicId(typePath, stringDefinition);
      }

      var definition = new FieldSymbolDefinition()
      {
         SymbolId = id,
         TypeId = typeId,
         
         RefType = fieldSymbol.RefKind.ToStorage(),
         
         HasConstantValue =  fieldSymbol.HasConstantValue,
         IsConst = fieldSymbol.IsConst,
         IsReadOnly = fieldSymbol.IsReadOnly,
         IsVolatile = fieldSymbol.IsVolatile,
         IsRequired =  fieldSymbol.IsRequired,
      };

      await batch.FieldSymbolWriter.Write(id, definition);
      return true;
   }
}
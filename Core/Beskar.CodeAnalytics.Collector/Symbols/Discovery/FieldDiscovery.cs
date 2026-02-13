using Beskar.CodeAnalytics.Collector.Extensions;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Symbols.Discovery;

public static class FieldDiscovery
{
   public static async Task<bool> Discover(DiscoverContext context, uint id)
   {
      if (context.Symbol is not IFieldSymbol fieldSymbol)
      {
         return false;
      }

      var batch = context.DiscoveryBatch;
      
      uint typeId = 0;
      if (UniqueIdentifier.Create(fieldSymbol.Type) is { } typePath)
      {
         var stringDefinition = batch.StringDefinitions.GetStringFileView(typePath);
         typeId = batch.Identifiers.GenerateIdentifier(typePath, stringDefinition);
      }

      var definition = new FieldSymbolSpec()
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
using Beskar.CodeAnalytics.Collector.Extensions;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Storage.Entities.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Symbols.Discovery;

public static class ParameterDiscovery
{
   public static async Task<bool> Discover(DiscoverContext context, ulong id)
   {
      if (context.Symbol is not IParameterSymbol parameter)
      {
         return false;
      }

      var batch = context.DiscoveryBatch;

      ulong typeId = 0;
      if (UniqueIdentifier.Create(parameter.Type) is { } typePath)
      {
         var stringDefinition = batch.StringDefinitions.GetStringDefinition(typePath);
         typeId = batch.Identifiers.GetDeterministicId(typePath, stringDefinition);
      }
      
      var parameterDefinition = new ParameterSymbolDefinition()
      {
         SymbolId = id,
         TypeId = typeId,
         Ordinal =  parameter.Ordinal,
         
         IsOptional = parameter.IsOptional,
         IsDiscard = parameter.IsDiscard,
         IsParamsArray = parameter.IsParamsArray,
         IsParamsCollection = parameter.IsParamsCollection,
         HasExplicitDefaultValue = parameter.HasExplicitDefaultValue,
         
         RefType = parameter.RefKind.ToStorage(),
         ScopeType = parameter.ScopedKind.ToStorage(),
      };
      
      await batch.ParameterSymbolWriter.Write(id, parameterDefinition);
      return true;
   }
}
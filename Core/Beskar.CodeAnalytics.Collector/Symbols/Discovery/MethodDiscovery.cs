using Beskar.CodeAnalytics.Collector.Extensions;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Symbols.Discovery;

public static class MethodDiscovery
{
   public static async Task<bool> Discover(DiscoverContext context, uint id)
   {
      if (context.Symbol is not IMethodSymbol methodSymbol)
      {
         return false;
      }
      
      var batch = context.DiscoveryBatch;

      uint overridenId = 0;
      var hasOverridden = false;
      
      if (methodSymbol.OverriddenMethod is not null
          && UniqueIdentifier.Create(methodSymbol.OverriddenMethod) is { } overriddenPath)
      {
         var stringDefinition = batch.StringDefinitions.GetStringFileView(overriddenPath);
         overridenId = batch.Identifiers.GenerateIdentifier(overriddenPath, stringDefinition);
         
         hasOverridden = true;
      }

      uint returnTypeId = 0;
      if (UniqueIdentifier.Create(methodSymbol.ReturnType) is { } returnTypePath)
      {
         var stringDefinition = batch.StringDefinitions.GetStringFileView(returnTypePath);
         returnTypeId = batch.Identifiers.GenerateIdentifier(returnTypePath, stringDefinition);
      }

      batch.WriteDiscoveryEdges(id, methodSymbol.Parameters, SymbolEdgeType.Parameter);
      batch.WriteDiscoveryEdges(id, methodSymbol.TypeParameters, SymbolEdgeType.TypeParameter);
      
      var methodDefinition = new MethodSymbolSpec()
      {
         SymbolId = id,
         OverriddenMethodId = overridenId,
         ReturnTypeId = returnTypeId,
         MethodType = methodSymbol.MethodKind.ToStorage(),
         
         Parameters = new StorageView<ParameterSymbolSpec>(-1, -1),
         TypeParameters = new StorageView<TypeParameterSymbolSpec>(-1, -1),
         
         HasOverriddenMethod = hasOverridden,
         IsAsync = methodSymbol.IsAsync,
         IsIterator = methodSymbol.IsIterator,
         IsReadOnly = methodSymbol.IsReadOnly,
         ReturnsByRef = methodSymbol.ReturnsByRef,
         ReturnsByRefReadonly = methodSymbol.ReturnsByRefReadonly,
         HasVoidReturn = methodSymbol.ReturnsVoid,
      };

      await batch.MethodSymbolWriter.Write(id, methodDefinition);
      return true;
   }
}
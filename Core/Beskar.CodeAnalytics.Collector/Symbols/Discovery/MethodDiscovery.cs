using Beskar.CodeAnalytics.Collector.Extensions;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Storage.Entities.Edges;
using Beskar.CodeAnalytics.Storage.Entities.Misc;
using Beskar.CodeAnalytics.Storage.Entities.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Symbols.Discovery;

public static class MethodDiscovery
{
   public static async Task<bool> Discover(DiscoverContext context, ulong id)
   {
      if (context.Symbol is not IMethodSymbol methodSymbol)
      {
         return false;
      }
      
      var batch = context.DiscoveryBatch;

      ulong overridenId = 0;
      var hasOverridden = false;
      
      if (methodSymbol.OverriddenMethod is not null
          && UniqueIdentifier.Create(methodSymbol.OverriddenMethod) is { } overriddenPath)
      {
         var stringDefinition = batch.StringDefinitions.GetStringDefinition(overriddenPath);
         overridenId = batch.Identifiers.GetDeterministicId(overriddenPath, stringDefinition);
         
         hasOverridden = true;
      }

      ulong returnTypeId = 0;
      if (UniqueIdentifier.Create(methodSymbol.ReturnType) is { } returnTypePath)
      {
         var stringDefinition = batch.StringDefinitions.GetStringDefinition(returnTypePath);
         returnTypeId = batch.Identifiers.GetDeterministicId(returnTypePath, stringDefinition);
      }

      batch.WriteDiscoveryEdges(id, methodSymbol.Parameters, EdgeType.Parameter);
      batch.WriteDiscoveryEdges(id, methodSymbol.TypeParameters, EdgeType.TypeParameter);
      
      var methodDefinition = new MethodSymbolDefinition()
      {
         SymbolId = id,
         OverriddenMethodId = overridenId,
         ReturnTypeId = returnTypeId,
         MethodType = methodSymbol.MethodKind.ToStorage(),
         
         Parameters = new StorageSlice<ParameterSymbolDefinition>(-1, -1),
         TypeParameters = new StorageSlice<TypeParameterSymbolDefinition>(-1, -1),
         
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
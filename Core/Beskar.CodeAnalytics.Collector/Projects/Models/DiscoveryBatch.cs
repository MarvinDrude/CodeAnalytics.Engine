using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Options;
using Beskar.CodeAnalytics.Storage.Discovery.Writers;
using Beskar.CodeAnalytics.Storage.Entities.Edges;
using Beskar.CodeAnalytics.Storage.Entities.Symbols;
using Beskar.CodeAnalytics.Storage.Hashing;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Projects.Models;

public sealed class DiscoveryBatch : IDisposable
{
   public required StringDefinitionFileTracker StringDefinitions { get; init; }
   public required IdentifierGenerator Identifiers { get; init; }
   
   public required SymbolDiscoveryWriter<SymbolDefinition> SymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<TypeSymbolDefinition> TypeSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<NamedTypeSymbolDefinition> NamedTypeSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<ParameterSymbolDefinition> ParameterSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<TypeParameterSymbolDefinition> TypeParameterSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<MethodSymbolDefinition> MethodSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<FieldSymbolDefinition> FieldSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<PropertySymbolDefinition> PropertySymbolWriter { get; init; }
   public required EdgeDiscoveryWriter EdgeDiscoveryWriter { get; init; }
   
   public bool TryGetDeterministicId<TSymbol>(TSymbol? symbol, out ulong id)
      where TSymbol : ISymbol
   {
      if (symbol is null || UniqueIdentifier.Create(symbol) is not { } uniqueId)
      {
         id = 0;
         return false;
      }

      var stringDefinition = StringDefinitions.GetStringDefinition(uniqueId);
      id = Identifiers.GetDeterministicId(uniqueId, stringDefinition);
      
      return true;
   }

   public void WriteDiscoveryEdges<TSymbol>(ulong symbolId, ImmutableArray<TSymbol> targetSymbols, EdgeType type)
      where TSymbol : ISymbol
   {
      foreach (var symbol in targetSymbols)
      {
         WriteDiscoveryEdge(symbolId, symbol, type);
      }
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteDiscoveryEdge<TSymbol>(ulong symbolId, TSymbol? targetSymbol, EdgeType type)
      where TSymbol : ISymbol
   {
      if (TryGetDeterministicId(targetSymbol, out var targetId))
      {
         WriteDiscoveryEdge(symbolId, targetId, type);
      }
   }
   
   public void WriteDiscoveryEdge(ulong sourceId, ulong targetId, EdgeType type)
   {
      var edge = new EdgeDefinition()
      {
         Key = new EdgeKey(sourceId, targetId, type),
      };
      
      EdgeDiscoveryWriter.Write(ref edge);
   }
   
   public void Dispose()
   {
      StringDefinitions.Dispose();
      
      SymbolWriter.Dispose();
      TypeSymbolWriter.Dispose();
      NamedTypeSymbolWriter.Dispose();
      ParameterSymbolWriter.Dispose();
      TypeParameterSymbolWriter.Dispose();
      MethodSymbolWriter.Dispose();
      FieldSymbolWriter.Dispose();
      PropertySymbolWriter.Dispose();
      EdgeDiscoveryWriter.Dispose();
   }

   public static DiscoveryBatch CreateEmpty(CollectorOptions options)
   {
      return new DiscoveryBatch()
      {
         Identifiers = new IdentifierGenerator(),
         StringDefinitions = new StringDefinitionFileTracker(Path.Combine(options.OutputPath, _fileNameStrings)),
         
         SymbolWriter = new SymbolDiscoveryWriter<SymbolDefinition>(options.OutputPath),
         TypeSymbolWriter = new SymbolDiscoveryWriter<TypeSymbolDefinition>(options.OutputPath),
         NamedTypeSymbolWriter = new SymbolDiscoveryWriter<NamedTypeSymbolDefinition>(options.OutputPath),
         ParameterSymbolWriter = new SymbolDiscoveryWriter<ParameterSymbolDefinition>(options.OutputPath),
         TypeParameterSymbolWriter = new SymbolDiscoveryWriter<TypeParameterSymbolDefinition>(options.OutputPath),
         MethodSymbolWriter = new SymbolDiscoveryWriter<MethodSymbolDefinition>(options.OutputPath),
         FieldSymbolWriter = new SymbolDiscoveryWriter<FieldSymbolDefinition>(options.OutputPath),
         PropertySymbolWriter = new SymbolDiscoveryWriter<PropertySymbolDefinition>(options.OutputPath),
         EdgeDiscoveryWriter = new EdgeDiscoveryWriter(options.OutputPath)
      };
   }

   private const string _fileNameStrings = "strings.discovery.mmb";
}
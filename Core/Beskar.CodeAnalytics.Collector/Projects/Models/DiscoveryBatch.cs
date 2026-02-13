using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Options;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Hashing;
using Beskar.CodeAnalytics.Storage.Discovery.Writers;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Projects.Models;

public sealed class DiscoveryBatch : IDisposable
{
   public required StringFileWriter StringDefinitions { get; init; }
   public required IdentifierGenerator Identifiers { get; init; }
   
   public required SymbolDiscoveryWriter<SymbolSpec> SymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<TypeSymbolSpec> TypeSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<NamedTypeSymbolSpec> NamedTypeSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<ParameterSymbolSpec> ParameterSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<TypeParameterSymbolSpec> TypeParameterSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<MethodSymbolSpec> MethodSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<FieldSymbolSpec> FieldSymbolWriter { get; init; }
   public required SymbolDiscoveryWriter<PropertySymbolSpec> PropertySymbolWriter { get; init; }
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

   public DiscoveryResult CreateResult()
   {
      return new DiscoveryResult()
      {
         SymbolFilePath = SymbolWriter.FilePath,
         TypeSymbolFilePath = TypeSymbolWriter.FilePath,
         NamedTypeSymbolFilePath = NamedTypeSymbolWriter.FilePath,
         ParameterSymbolFilePath = ParameterSymbolWriter.FilePath,
         TypeParameterSymbolFilePath = TypeParameterSymbolWriter.FilePath,
         MethodSymbolFilePath = MethodSymbolWriter.FilePath,
         FieldSymbolFilePath = FieldSymbolWriter.FilePath,
         PropertySymbolFilePath = PropertySymbolWriter.FilePath,
         EdgeFilePath = EdgeDiscoveryWriter.FilePath,
      };
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
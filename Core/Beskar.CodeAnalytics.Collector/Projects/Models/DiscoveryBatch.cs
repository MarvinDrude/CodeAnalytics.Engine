using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Options;
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Discovery.Writers;
using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Beskar.CodeAnalytics.Data.Hashing;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Projects.Models;

public sealed class DiscoveryBatch : IAsyncDisposable
{
   public required CollectorOptions Options { get; init; }
   
   public required StringFileWriter StringDefinitions { get; init; }
   public required IdentifierGenerator Identifiers { get; init; }
   
   public required SymbolDiscoveryFileWriter<uint, SymbolSpec> SymbolWriter { get; init; }
   public required SymbolDiscoveryFileWriter<uint, TypeSymbolSpec> TypeSymbolWriter { get; init; }
   public required SymbolDiscoveryFileWriter<uint, NamedTypeSymbolSpec> NamedTypeSymbolWriter { get; init; }
   public required SymbolDiscoveryFileWriter<uint, ParameterSymbolSpec> ParameterSymbolWriter { get; init; }
   public required SymbolDiscoveryFileWriter<uint, TypeParameterSymbolSpec> TypeParameterSymbolWriter { get; init; }
   public required SymbolDiscoveryFileWriter<uint, MethodSymbolSpec> MethodSymbolWriter { get; init; }
   public required SymbolDiscoveryFileWriter<uint, FieldSymbolSpec> FieldSymbolWriter { get; init; }
   public required SymbolDiscoveryFileWriter<uint, PropertySymbolSpec> PropertySymbolWriter { get; init; }
   public required SymbolDiscoveryFileWriter<SymbolEdgeKey, SymbolEdgeSpec> EdgeWriter { get; init; }
   
   public required SymbolDiscoveryFileWriter<uint, SolutionSpec> SolutionWriter { get; init; }
   public required SymbolDiscoveryFileWriter<uint, ProjectSpec> ProjectWriter { get; init; }
   public required SymbolDiscoveryFileWriter<uint, FileSpec> FileWriter { get; init; }
   
   public bool TryGetDeterministicId<TSymbol>(TSymbol? symbol, out uint id)
      where TSymbol : ISymbol
   {
      if (symbol is null || UniqueIdentifier.Create(symbol) is not { } uniqueId)
      {
         id = 0;
         return false;
      }

      var stringDefinition = StringDefinitions.GetStringFileView(uniqueId);
      id = Identifiers.GenerateIdentifier(uniqueId, stringDefinition.Offset);
      
      return true;
   }

   public void WriteDiscoveryEdges<TSymbol>(uint symbolId, ImmutableArray<TSymbol> targetSymbols, SymbolEdgeType type)
      where TSymbol : ISymbol
   {
      foreach (var symbol in targetSymbols)
      {
         WriteDiscoveryEdge(symbolId, symbol, type);
      }
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public void WriteDiscoveryEdge<TSymbol>(uint symbolId, TSymbol? targetSymbol, SymbolEdgeType type)
      where TSymbol : ISymbol
   {
      if (TryGetDeterministicId(targetSymbol, out var targetId))
      {
         WriteDiscoveryEdge(symbolId, targetId, type);
      }
   }
   
   public void WriteDiscoveryEdge(uint sourceId, uint targetId, SymbolEdgeType type)
   {
      var edge = new SymbolEdgeSpec()
      {
         SourceSymbolId = sourceId,
         TargetSymbolId = targetId,
         Type = type
      };
      
      var task = EdgeWriter.Write(new SymbolEdgeKey(sourceId, targetId, type), edge);
      if (!task.IsCompletedSuccessfully)
      {
         throw new InvalidOperationException();
      }
   }
   
   public async ValueTask DisposeAsync()
   {
      await SymbolWriter.DisposeAsync();
      await TypeSymbolWriter.DisposeAsync();
      await NamedTypeSymbolWriter.DisposeAsync();
      await ParameterSymbolWriter.DisposeAsync();
      await TypeParameterSymbolWriter.DisposeAsync();
      await MethodSymbolWriter.DisposeAsync();
      await FieldSymbolWriter.DisposeAsync();
      await PropertySymbolWriter.DisposeAsync();
      await EdgeWriter.DisposeAsync();
      
      await SolutionWriter.DisposeAsync();
      await ProjectWriter.DisposeAsync();
      await FileWriter.DisposeAsync();
   }

   public Dictionary<FileId, string> GetFileNames()
   {
      return new Dictionary<FileId, string>()
      {
         [FileIds.Symbol] = SymbolWriter.FileName,
         [FileIds.TypeSymbol] = TypeSymbolWriter.FileName,
         [FileIds.NamedTypeSymbol] = NamedTypeSymbolWriter.FileName,
         [FileIds.ParameterSymbol] = ParameterSymbolWriter.FileName,
         [FileIds.TypeParameterSymbol] = TypeParameterSymbolWriter.FileName,
         [FileIds.MethodSymbol] = MethodSymbolWriter.FileName,
         [FileIds.FieldSymbol] = FieldSymbolWriter.FileName,
         [FileIds.PropertySymbol] = PropertySymbolWriter.FileName,
         [FileIds.EdgeSymbol] = EdgeWriter.FileName,
         
         [FileIds.Solution] = SolutionWriter.FileName,
         [FileIds.Project] = ProjectWriter.FileName,
         [FileIds.File] = FileWriter.FileName,
      };
   }
   
   public static DiscoveryBatch CreateEmpty(CollectorOptions options)
   {
      return new DiscoveryBatch()
      {
         Options = options,
         Identifiers = new IdentifierGenerator(),
         StringDefinitions = new StringFileWriter(Path.Combine(options.OutputPath, FileNames.StringPool)),
         
         SymbolWriter = new SymbolDiscoveryFileWriter<uint, SymbolSpec>(options.OutputPath),
         TypeSymbolWriter = new SymbolDiscoveryFileWriter<uint, TypeSymbolSpec>(options.OutputPath),
         NamedTypeSymbolWriter = new SymbolDiscoveryFileWriter<uint, NamedTypeSymbolSpec>(options.OutputPath),
         ParameterSymbolWriter = new SymbolDiscoveryFileWriter<uint, ParameterSymbolSpec>(options.OutputPath),
         TypeParameterSymbolWriter = new SymbolDiscoveryFileWriter<uint, TypeParameterSymbolSpec>(options.OutputPath),
         MethodSymbolWriter = new SymbolDiscoveryFileWriter<uint, MethodSymbolSpec>(options.OutputPath),
         FieldSymbolWriter = new SymbolDiscoveryFileWriter<uint, FieldSymbolSpec>(options.OutputPath),
         PropertySymbolWriter = new SymbolDiscoveryFileWriter<uint, PropertySymbolSpec>(options.OutputPath),
         EdgeWriter = new SymbolDiscoveryFileWriter<SymbolEdgeKey, SymbolEdgeSpec>(options.OutputPath),
         
         SolutionWriter = new SymbolDiscoveryFileWriter<uint, SolutionSpec>(options.OutputPath),
         ProjectWriter = new SymbolDiscoveryFileWriter<uint, ProjectSpec>(options.OutputPath),
         FileWriter = new SymbolDiscoveryFileWriter<uint, FileSpec>(options.OutputPath),
      };
   }
}
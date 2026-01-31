using Beskar.CodeAnalytics.Collector.Options;
using Beskar.CodeAnalytics.Storage.Discovery.Writers;
using Beskar.CodeAnalytics.Storage.Entities.Symbols;
using Beskar.CodeAnalytics.Storage.Hashing;

namespace Beskar.CodeAnalytics.Collector.Projects.Models;

public sealed class DiscoveryBatch : IDisposable
{
   public required StringDefinitionFileTracker StringDefinitions { get; init; }
   public required IdentifierGenerator Identifiers { get; init; }
   
   public required SymbolDiscoveryWriter<SymbolDefinition> SymbolWriter { get; init; }

   public void Dispose()
   {
      StringDefinitions.Dispose();
      SymbolWriter.Dispose();
   }

   public static DiscoveryBatch CreateEmpty(CollectorOptions options)
   {
      return new DiscoveryBatch()
      {
         Identifiers = new IdentifierGenerator(),
         StringDefinitions = new StringDefinitionFileTracker(Path.Combine(options.OutputPath, _fileNameStrings)),
         
         SymbolWriter = new SymbolDiscoveryWriter<SymbolDefinition>(options.OutputPath),
      };
   }

   private const string _fileNameStrings = "strings.discovery.mmb";
}
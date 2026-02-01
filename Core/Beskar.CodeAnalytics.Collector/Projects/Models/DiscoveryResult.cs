namespace Beskar.CodeAnalytics.Collector.Projects.Models;

public sealed class DiscoveryResult
{
   public required string SymbolFilePath { get; set; }
   public required string TypeSymbolFilePath { get; set; }
   public required string NamedTypeSymbolFilePath { get; set; }
   public required string ParameterSymbolFilePath { get; set; }
   public required string TypeParameterSymbolFilePath { get; set; }
   public required string MethodSymbolFilePath { get; set; }
   public required string FieldSymbolFilePath { get; set; }
   public required string PropertySymbolFilePath { get; set; }
   public required string EdgeFilePath { get; set; }
}
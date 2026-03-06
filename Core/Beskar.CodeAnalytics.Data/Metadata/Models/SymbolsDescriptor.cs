using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Models;

public sealed class SymbolsDescriptor
{
   public required FieldSymbolSpecDescriptor Fields { get; set; }
   
   public required MethodSymbolSpecDescriptor Methods { get; set; }
   
   public required PropertySymbolSpecDescriptor Properties { get; set; }
   
   public required SymbolSpecDescriptor Symbols { get; set; }
   
   public required TypeSymbolSpecDescriptor Types { get; set; }
   
   public required TypeParameterSymbolSpecDescriptor TypeParameters { get; set; }
   
   public required ParameterSymbolSpecDescriptor Parameters { get; set; }
   
   public required NamedTypeSymbolSpecDescriptor NamedTypes { get; set; }
   
   public async Task Initialize(DatabaseDescriptor database)
   {
      await Fields.Initialize(database);
      await Methods.Initialize(database);
      await Properties.Initialize(database);
      await Symbols.Initialize(database);
      await Types.Initialize(database);
      await TypeParameters.Initialize(database);
      await Parameters.Initialize(database);
      await NamedTypes.Initialize(database);
   }
}
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;

public sealed class NamedTypeSymbolSpecDescriptorSerializer : SpecDescriptorSerializer<NamedTypeSymbolSpecDescriptor, NamedTypeSymbolSpec>
{
   protected override NamedTypeSymbolSpecDescriptor CreateDescriptor(string fileName)
   {
      return new NamedTypeSymbolSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
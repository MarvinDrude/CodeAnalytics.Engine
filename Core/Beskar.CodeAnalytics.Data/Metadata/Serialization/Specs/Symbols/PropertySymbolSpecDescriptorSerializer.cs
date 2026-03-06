using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;

public sealed class PropertySymbolSpecDescriptorSerializer : SpecDescriptorSerializer<PropertySymbolSpecDescriptor, PropertySymbolSpec>
{
   protected override PropertySymbolSpecDescriptor CreateDescriptor(string fileName)
   {
      return new PropertySymbolSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
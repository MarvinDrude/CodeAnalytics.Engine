using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;

public sealed class ParameterSymbolSpecDescriptorSerializer : SpecDescriptorSerializer<ParameterSymbolSpecDescriptor, ParameterSymbolSpec>
{
   protected override ParameterSymbolSpecDescriptor CreateDescriptor(string fileName)
   {
      return new ParameterSymbolSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
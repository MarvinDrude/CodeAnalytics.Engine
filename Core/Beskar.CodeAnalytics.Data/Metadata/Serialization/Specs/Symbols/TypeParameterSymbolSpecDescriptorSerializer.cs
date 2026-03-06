using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;

public sealed class TypeParameterSymbolSpecDescriptorSerializer : SpecDescriptorSerializer<TypeParameterSymbolSpecDescriptor, TypeParameterSymbolSpec>
{
   protected override TypeParameterSymbolSpecDescriptor CreateDescriptor(string fileName)
   {
      return new TypeParameterSymbolSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;

public sealed class TypeSymbolSpecDescriptorSerializer : SpecDescriptorSerializer<TypeSymbolSpecDescriptor, TypeSymbolSpec>
{
   protected override TypeSymbolSpecDescriptor CreateDescriptor(string fileName)
   {
      return new TypeSymbolSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
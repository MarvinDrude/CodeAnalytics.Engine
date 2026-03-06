using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;

public sealed class FieldSymbolSpecDescriptorSerializer : SpecDescriptorSerializer<FieldSymbolSpecDescriptor, FieldSymbolSpec>
{
   protected override FieldSymbolSpecDescriptor CreateDescriptor(string fileName)
   {
      return new FieldSymbolSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
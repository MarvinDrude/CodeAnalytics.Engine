
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;

public sealed class SymbolSpecDescriptorSerializer : SpecDescriptorSerializer<SymbolSpecDescriptor, SymbolSpec>
{
   protected override SymbolSpecDescriptor CreateDescriptor(string fileName)
   {
      return new SymbolSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
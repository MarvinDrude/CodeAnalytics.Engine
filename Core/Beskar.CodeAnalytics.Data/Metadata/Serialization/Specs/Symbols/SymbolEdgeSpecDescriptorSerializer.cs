using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;

public sealed class SymbolEdgeSpecDescriptorSerializer : SpecDescriptorSerializer<SymbolEdgeSpecDescriptor, SymbolEdgeSpec>
{
   protected override SymbolEdgeSpecDescriptor CreateDescriptor(string fileName)
   {
      return new SymbolEdgeSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
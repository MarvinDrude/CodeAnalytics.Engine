using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Specs;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs;

public sealed class SymbolLocationSpecDescriptorSerializer : SpecDescriptorSerializer<SymbolLocationSpecDescriptor, SymbolLocationSpec>
{
   protected override SymbolLocationSpecDescriptor CreateDescriptor(string fileName)
   {
      return new SymbolLocationSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs.Symbols;

public sealed class MethodSymbolDescriptorSerializer : SpecDescriptorSerializer<MethodSymbolSpecDescriptor, MethodSymbolSpec>
{
   protected override MethodSymbolSpecDescriptor CreateDescriptor(string fileName)
   {
      return new MethodSymbolSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
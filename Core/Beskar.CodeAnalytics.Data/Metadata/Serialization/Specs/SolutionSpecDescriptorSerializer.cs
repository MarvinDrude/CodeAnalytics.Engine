using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Metadata.Specs;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs;

public sealed class SolutionSpecDescriptorSerializer : SpecDescriptorSerializer<SolutionSpecDescriptor, SolutionSpec>
{
   protected override SolutionSpecDescriptor CreateDescriptor(string fileName)
   {
      return new SolutionSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
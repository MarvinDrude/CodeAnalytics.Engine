using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Metadata.Specs;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs;

public sealed class ProjectSpecDescriptorSerializer : SpecDescriptorSerializer<ProjectSpecDescriptor, ProjectSpec>
{
   protected override ProjectSpecDescriptor CreateDescriptor(string fileName)
   {
      return new ProjectSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
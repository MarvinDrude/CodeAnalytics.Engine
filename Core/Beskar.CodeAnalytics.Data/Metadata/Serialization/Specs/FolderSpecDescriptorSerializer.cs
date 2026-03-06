using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Metadata.Specs;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs;

public sealed class FolderSpecDescriptorSerializer : SpecDescriptorSerializer<FolderSpecDescriptor, FolderSpec>
{
   protected override FolderSpecDescriptor CreateDescriptor(string fileName)
   {
      return new FolderSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
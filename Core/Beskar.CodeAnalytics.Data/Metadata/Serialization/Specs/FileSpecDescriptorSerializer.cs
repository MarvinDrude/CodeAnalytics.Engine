using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Metadata.Specs;

namespace Beskar.CodeAnalytics.Data.Metadata.Serialization.Specs;

public sealed class FileSpecDescriptorSerializer : SpecDescriptorSerializer<FileSpecDescriptor, FileSpec>
{
   protected override FileSpecDescriptor CreateDescriptor(string fileName)
   {
      return new FileSpecDescriptor()
      {
         FileName = fileName
      };
   }
}
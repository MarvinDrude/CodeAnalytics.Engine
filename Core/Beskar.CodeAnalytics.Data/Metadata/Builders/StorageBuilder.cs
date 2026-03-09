using Beskar.CodeAnalytics.Data.Metadata.Storage;

namespace Beskar.CodeAnalytics.Data.Metadata.Builders;

public sealed class StorageBuilder
{
   public List<StorageFileDescriptor> Files { get; } = [];

   public StorageDescriptor Build()
   {
      return new StorageDescriptor { Files = Files };
   }
}
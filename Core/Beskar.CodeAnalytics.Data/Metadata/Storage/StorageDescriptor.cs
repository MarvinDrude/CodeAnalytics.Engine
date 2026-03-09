namespace Beskar.CodeAnalytics.Data.Metadata.Storage;

public sealed class StorageDescriptor
{
   public required List<StorageFileDescriptor> Files { get; set; }
}
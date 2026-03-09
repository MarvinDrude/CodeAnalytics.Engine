using Beskar.CodeAnalytics.Data.Enums.Storage;

namespace Beskar.CodeAnalytics.Data.Metadata.Storage;

public sealed class StorageFileDescriptor
{
   public required string ParentName { get; set; }
   
   public required string FileName { get; set; }
   public required string Name { get; set; }
   
   public required StorageFileKind Kind { get; set; }
   public required DateTimeOffset LastModified { get; set; }
   
   public required ulong ByteCount { get; set; }
   public required ulong RowCount { get; set; }
}
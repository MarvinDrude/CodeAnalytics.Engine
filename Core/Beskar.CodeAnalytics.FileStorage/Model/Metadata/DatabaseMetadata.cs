namespace Beskar.CodeAnalytics.FileStorage.Model.Metadata;

public sealed class DatabaseMetadata
{
   public required string Name { get; set; }
   public required string RootDirectory { get; set; }

   public Dictionary<string, TableMetadata> Tables { get; set; } = [];
}
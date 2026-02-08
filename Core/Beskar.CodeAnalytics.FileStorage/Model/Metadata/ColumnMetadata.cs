namespace Beskar.CodeAnalytics.FileStorage.Model.Metadata;

public sealed class ColumnMetadata
{
   public required string Name { get; set; }
   public required int StructOffset  { get; set; }
}
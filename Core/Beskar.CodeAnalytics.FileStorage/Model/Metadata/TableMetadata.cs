namespace Beskar.CodeAnalytics.FileStorage.Model.Metadata;

public sealed class TableMetadata
{
   public required string TableName { get; set; }
   public required string FileNameTemplate { get; set; }
   
   public int PageSize { get; set; }
   public ulong PageCount { get; set; }
   public ulong ItemCount { get; set; }
   
   
}
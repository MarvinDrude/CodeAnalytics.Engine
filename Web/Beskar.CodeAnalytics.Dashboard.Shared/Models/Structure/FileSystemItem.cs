namespace Beskar.CodeAnalytics.Dashboard.Shared.Models.Structure;

public abstract class FileSystemItem
{
   public required uint Id { get; set; }
   
   public required string Name { get; set; }
   
   public bool IsHighlighted { get; set; }
}
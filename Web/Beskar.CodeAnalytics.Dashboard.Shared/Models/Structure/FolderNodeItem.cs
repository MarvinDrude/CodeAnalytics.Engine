namespace Beskar.CodeAnalytics.Dashboard.Shared.Models.Structure;

public sealed class FolderNodeItem : FileSystemItem
{
   public List<FileSystemItem> Children { get; set; } = [];

   public bool IsExpanded { get; set; }
   
   public bool IsLoaded { get; set; }
}
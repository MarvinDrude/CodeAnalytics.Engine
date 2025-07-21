using CodeAnalytics.Web.Common.Enums.Explorer;

namespace CodeAnalytics.Web.Common.Models.Explorer;

public sealed class ExplorerTreeItem
{
   public bool IsExpanded { get; set; }
   public bool IsFolder => Type is ExplorerTreeItemType.Folder;
   public bool IsSelected  { get; set; }
   
   public ExplorerTreeItemType Type { get; set; }
   
   public required string Name { get; set; }
   public required string Path { get; set; }

   public List<ExplorerTreeItem> Children { get; set; } = [];

   public ExplorerTreeItem Clone()
   {
      return new ExplorerTreeItem()
      {
         IsExpanded = IsExpanded,
         IsSelected = IsSelected,
         Type = Type,
         Name = Name,
         Path = Path,
         Children = Clone(Children)
      };
   }

   public static List<ExplorerTreeItem> Clone(List<ExplorerTreeItem> explorerTreeItems)
   {
      return [..explorerTreeItems.Select(x => x.Clone())];
   }
}
using System.Runtime.InteropServices;
using CodeAnalytics.Web.Common.Enums.Explorer;

namespace CodeAnalytics.Web.Common.Models.Explorer;

[StructLayout(LayoutKind.Auto)]
public readonly struct ExplorerFlatTreeItem
{
   public ExplorerTreeItemType Type { get; init; }
   public string Name { get; init; }
   public string[] Path { get; init; }

   public ExplorerFlatTreeItem(
      ExplorerTreeItemType type, 
      string name, 
      string[] path)
   {
      Type = type;
      Name = name;
      Path = path;
   }
}
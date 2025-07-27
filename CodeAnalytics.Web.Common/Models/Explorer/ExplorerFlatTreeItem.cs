using System.Runtime.InteropServices;
using CodeAnalytics.Web.Common.Enums.Explorer;

namespace CodeAnalytics.Web.Common.Models.Explorer;

[StructLayout(LayoutKind.Auto)]
public readonly struct ExplorerFlatTreeItem
{
   public readonly ExplorerTreeItemType Type;
   public readonly string Name;
   public readonly string[] Path;

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
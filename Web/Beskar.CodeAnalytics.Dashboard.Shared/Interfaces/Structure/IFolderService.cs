using Beskar.CodeAnalytics.Dashboard.Shared.Models.Structure;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Structure;

public interface IFolderService
{
   public List<FileSystemItem> GetNodesByParent(FolderNodeItem parent);
   
   public List<FileSystemItem> GetNodesByParentId(uint parentId);

   public List<FileSystemItem> GetRootNodes();
}
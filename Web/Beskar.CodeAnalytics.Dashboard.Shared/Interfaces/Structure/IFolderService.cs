using Beskar.CodeAnalytics.Dashboard.Shared.Models.Structure;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Structure;

public interface IFolderService
{
   public List<FileSystemItem> GetNodesByParent(FolderNodeItem parent);
   
   public List<FileSystemItem> GetNodesByParentId(uint parentId);

   public List<FileSystemItem> GetRootNodes(uint childIdExpandTo, bool isFolder, bool highlight = true);

   public FileSystemItem? ExpandRootNodes(List<FileSystemItem> roots, uint childIdExpandTo, bool isFolder, bool highlight = true);

   public List<FileSystemItem> GetRootNodes(int depth);
}
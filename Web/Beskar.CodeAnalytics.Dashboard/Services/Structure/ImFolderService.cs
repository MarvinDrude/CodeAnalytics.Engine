using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Services;
using Beskar.CodeAnalytics.Dashboard.Shared.Interfaces.Structure;
using Beskar.CodeAnalytics.Dashboard.Shared.Models.Structure;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Me.Memory.Buffers;

namespace Beskar.CodeAnalytics.Dashboard.Services.Structure;

public sealed class ImFolderService(IDatabaseProvider dbProvider) : IFolderService
{
   private readonly IDatabaseProvider _databaseProvider = dbProvider;

   public List<FileSystemItem> GetNodesByParent(FolderNodeItem parent)
   {
      var children = GetNodesByParentId(parent.Id);
      parent.Children = children;
      parent.IsLoaded = true;
      
      return children;
   }
   
   public List<FileSystemItem> GetNodesByParentId(uint parentId)
   {
      var result = new List<FileSystemItem>(10);
      var db = _databaseProvider.GetDescriptor();
      
      using var fileIdsOwner = db.Edges.GetTargetIds(parentId, SymbolEdgeType.FolderToFile);
      using var folderIdsOwner = db.Edges.GetTargetIds(parentId, SymbolEdgeType.FolderToFolder);

      var fileIds = fileIdsOwner.WrittenSpan;
      fileIds.Sort();
      var folderIds = folderIdsOwner.WrittenSpan;
      folderIds.Sort();
      
      using var foldersOwner = db.Structure.Folders.GetReader().GetSpecsBySortedIds(folderIds);
      using var filesOwner = db.Structure.Files.GetReader().GetSpecsBySortedIds(fileIds);
      
      var folders = foldersOwner.WrittenSpan;
      var files = filesOwner.WrittenSpan;
      
      using var namesOwner = new SpanOwner<StringFileView>(folders.Length + files.Length);
      var names = namesOwner.Span;
      var offset = 0;

      foreach (var folder in folders)
      { names[offset++] = folder.Name; }

      foreach (var file in files)
      { names[offset++] = file.Name; }
      
      var strPool = db.StringPool.Reader;
      var fileNames = strPool.GetStrings(names);

      foreach (ref var folder in folders)
      {
         result.Add(new FolderNodeItem()
         {
            Id = folder.Id,
            Name = fileNames[folder.Name],
            IsExpanded = false
         });
      }

      foreach (var file in files)
      {
         result.Add(new FileNodeItem()
         {
            Id = file.Id,
            Name = fileNames[file.Name],
            Kind = file.Kind
         });
      }

      return result;
   }

   public List<FileSystemItem> GetRootNodes(uint childIdExpandTo, bool isFolder, bool highlight = true)
   {
      var provider = _databaseProvider.GetDescriptor().Structure.RootFolderId;
      var roots = GetNodesByParentId(provider);

      using var pathIds = GetPathToRoot(childIdExpandTo, isFolder);
      
      var path = pathIds.WrittenSpan;
      if (path.Length is 0 or 1) return roots;

      var current = roots;
      for (var e = path.Length - 2; e >= 0; e--)
      {
         var folderId = path[e];
         var node = current.OfType<FolderNodeItem>().FirstOrDefault(x => x.Id == folderId);

         if (node != null)
         {
            var children = GetNodesByParent(node);
            node.IsExpanded = true;
            
            current = children;
         }
      }
      
      var highlightNode = current.FirstOrDefault(x => x.Id == childIdExpandTo);
      highlightNode?.IsHighlighted = highlight;

      return roots;
   }

   public ArrayBuilderResult<uint> GetPathToRoot(uint childId, bool isFolder)
   {
      var rootId = _databaseProvider.GetDescriptor().Structure.RootFolderId;
      var builder = new ArrayBuilder<uint>(20);

      try
      {
         var parentId = GetParentFolderId(childId, isFolder);
         builder.Add(parentId);
         
         while (parentId != rootId)
         {
            parentId = GetParentFolderId(parentId, true);
            builder.Add(parentId);
         }
      }
      catch (Exception)
      {
         builder.Dispose();
         throw;
      }
      
      return builder;
   }

   public List<FileSystemItem> GetRootNodes(int depth)
   {
      var rootFolderId = _databaseProvider.GetDescriptor().Structure.RootFolderId;
      var rootNodes = GetNodesByParentId(rootFolderId);

      foreach (var node in rootNodes)
      {
         ExpandNodeRecursive(node, depth);
      }
      
      return rootNodes;
   }
   
   private void ExpandNodeRecursive(FileSystemItem item, int depth)
   {
      if (depth <= 0 || item is not FolderNodeItem folder)
      {
         return;
      }

      folder.IsExpanded = true;
   
      var children = GetNodesByParentId(folder.Id);
      folder.Children = children; 

      foreach (var child in children)
      {
         ExpandNodeRecursive(child, depth - 1);
      }
   }
   
   private uint GetParentFolderId(uint childId, bool isFolder)
   {
      var db = _databaseProvider.GetDescriptor();
      var parentId = isFolder 
         ? db.Structure.Folders.GetReader().GetSpecById(childId).ParentId 
         : db.Structure.Files.GetReader().GetSpecById(childId).ParentId;
      
      return parentId;
   }
}
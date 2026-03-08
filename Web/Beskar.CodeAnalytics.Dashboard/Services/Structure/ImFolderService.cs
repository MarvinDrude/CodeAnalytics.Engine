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

   public List<FileSystemItem> GetRootNodes()
   {
      var provider = _databaseProvider.GetDescriptor().Structure.RootFolderId;
      return GetNodesByParentId(provider);
   }
}
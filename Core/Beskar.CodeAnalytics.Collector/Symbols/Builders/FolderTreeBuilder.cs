using System.Collections.Concurrent;
using System.Text;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Structure;

namespace Beskar.CodeAnalytics.Collector.Symbols.Builders;

public sealed class FolderTreeBuilder
{
   private readonly ConcurrentDictionary<string, uint> _nodes = new();
   private readonly Lock _lock = new();

   private FolderNode? _root;
   
   public uint GetOrCreateFolder(DiscoveryBatch batch, string path)
   {
      if (_nodes.TryGetValue(path, out var id))
      {
         return id;
      }
      
      lock (_lock)
      {
         // double-check
         if (_nodes.TryGetValue(path, out id))
         {
            return id;
         }
         
         var root = GetOrCreateRoot(batch);
         var segments = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

         var currentFullPath = new StringBuilder((int)(path.Length * 1.5f));
         currentFullPath.Append("");

         var parent = root;
         for (var e = 0; e < segments.Length; e++)
         {
            var segmentName = segments[e];
            
            if (e > 0) currentFullPath.Append('/');
            currentFullPath.Append(segmentName);

            var currentPath = currentFullPath.ToString();
            var folderId = _nodes.GetOrAdd(currentFullPath.ToString(), _ =>
            {
               var stringView = batch.StringDefinitions.GetStringFileView(currentPath);
               var folderId = batch.Identifiers.GenerateIdentifier(currentPath, stringView);

               return folderId;
            });

            var node = new FolderNode()
            {
               Id = folderId,
               ParentId = parent.Id,
               Name = segmentName
            };
            
            if (!parent.Children.TryGetValue(node, out var existingNode))
            {
               parent.Children.Add(node);
               
               var folderStringView = batch.StringDefinitions.GetStringFileView(currentPath);
               var folderNameView = batch.StringDefinitions.GetStringFileView(segmentName);
               
               var task = batch.FolderWriter.Write(node.Id, new FolderSpec()
               {
                  Id = node.Id,
                  ParentId = parent.Id,
                  IsRoot = false,
                  IsVirtual = false,
                  FullPath = folderStringView,
                  Name = folderNameView,
                  
                  Files = new StorageView<FileSpec>(-1, -1),
                  Projects = new StorageView<uint>(-1, -1),
                  SubFolders = new StorageView<FolderSpec>(-1, -1)
               });
               if (!task.IsCompletedSuccessfully) throw new InvalidOperationException();
            }
            else
            {
               node = existingNode;
            }
            
            parent = node;
         }

         return parent.Id;
      }
   }

   private FolderNode GetOrCreateRoot(DiscoveryBatch batch)
   {
      if (_root is not null)
      {
         return _root;
      }
      
      const string rootPathName = "/::root-path::";
      var stringView = batch.StringDefinitions.GetStringFileView(rootPathName);
      var id = batch.Identifiers.GenerateIdentifier(rootPathName, stringView);
      
      _root = new FolderNode()
      {
         Id = id,
         ParentId = 0,
         Name = rootPathName
      };
      
      var task = batch.FolderWriter.Write(id, new FolderSpec()
      {
         Id = id,
         ParentId = 0,
         IsRoot = true,
         FullPath = stringView,
         Name = stringView,
                  
         Files = new StorageView<FileSpec>(-1, -1),
         Projects = new StorageView<uint>(-1, -1),
         SubFolders = new StorageView<FolderSpec>(-1, -1)
      });
      
      return !task.IsCompletedSuccessfully 
         ? throw new InvalidOperationException() : _root;
   }

   private sealed class FolderNode : IEquatable<FolderNode>
   {
      public required uint Id { get; init; }
      public required uint ParentId { get; init; }
      
      public required string Name { get; init; }
      public HashSet<FolderNode> Children { get; set; } = [];
      
      public bool Equals(FolderNode? other)
      {
         if (other is null) return false;
         if (ReferenceEquals(this, other)) return true;
         
         return Id == other.Id && ParentId == other.ParentId && Name == other.Name;
      }

      public override bool Equals(object? obj)
      {
         return ReferenceEquals(this, obj) || obj is FolderNode other && Equals(other);
      }

      public override int GetHashCode()
      {
         return HashCode.Combine(Id, ParentId, Name);
      }
   }
}
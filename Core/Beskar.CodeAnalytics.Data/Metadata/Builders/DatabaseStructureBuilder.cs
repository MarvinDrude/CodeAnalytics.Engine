using Beskar.CodeAnalytics.Data.Metadata.Models;
using Beskar.CodeAnalytics.Data.Metadata.Specs;

namespace Beskar.CodeAnalytics.Data.Metadata.Builders;

public sealed class DatabaseStructureBuilder
{
   private uint _rootFolderId;

   private string? _fileNameFolderSpec;

   public DatabaseStructureBuilder WithRootFolderId(uint id)
   {
      _rootFolderId = id;
      return this;
   }

   public DatabaseStructureBuilder WithFolderSpec(string fileName)
   {
      _fileNameFolderSpec = fileName;
      return this;
   }
   
   public StructureDescriptor Build()
   {
      return new StructureDescriptor()
      {
         RootFolderId = _rootFolderId,
         Folders = new FolderSpecDescriptor()
         {
            FileName = _fileNameFolderSpec ?? throw new InvalidOperationException("Folder spec is not set")
         }
      };
   }
}
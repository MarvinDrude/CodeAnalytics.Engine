using Beskar.CodeAnalytics.Data.Metadata.Models;
using Beskar.CodeAnalytics.Data.Metadata.Specs;

namespace Beskar.CodeAnalytics.Data.Metadata.Builders;

public sealed class DatabaseStructureBuilder
{
   private uint _rootFolderId;

   private string? _fileNameFolderSpec;
   private string? _fileNameFileSpec;
   private string? _fileNameSolutionSpec;
   private string? _fileNameProjectSpec;
   private string? _fileNameSyntaxFile;
   private string? _fileNameSymbolLocationSpec;
   
   public DatabaseStructureBuilder WithRootFolderId(uint id)
   {
      _rootFolderId = id;
      return this;
   }

   public DatabaseStructureBuilder WithFileSpec(string fileName)
   {
      _fileNameFileSpec = fileName;
      return this;
   }
   
   public DatabaseStructureBuilder WithSolutionSpec(string fileName)
   {
      _fileNameSolutionSpec = fileName;
      return this;
   }

   public DatabaseStructureBuilder WithProjectSpec(string fileName)
   {
      _fileNameProjectSpec = fileName;
      return this;
   }
   
   public DatabaseStructureBuilder WithSyntaxFile(string fileName)
   {
      _fileNameSyntaxFile = fileName;
      return this;
   }
   
   public DatabaseStructureBuilder WithSymbolLocationSpec(string fileName)
   {
      _fileNameSymbolLocationSpec = fileName;
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
            FileName = _fileNameFolderSpec 
               ?? throw new InvalidOperationException("Folder spec is not set")
         },
         Files = new FileSpecDescriptor()
         {
            FileName = _fileNameFileSpec 
               ?? throw new InvalidOperationException("File spec is not set")
         },
         Projects = new ProjectSpecDescriptor()
         {
            FileName = _fileNameProjectSpec 
               ?? throw new InvalidOperationException("Project spec is not set")
         },
         Solutions = new SolutionSpecDescriptor()
         {
            FileName = _fileNameSolutionSpec 
               ?? throw new InvalidOperationException("Solution spec is not set")
         },
         SymbolLocations = new SymbolLocationSpecDescriptor()
         {
            FileName = _fileNameSymbolLocationSpec 
               ?? throw new InvalidOperationException("Symbol location spec is not set")
         },
         SyntaxFiles = new SyntaxFileDescriptor()
         {
            FileName = _fileNameSyntaxFile 
               ?? throw new InvalidOperationException("Syntax file is not set")
         }
      };
   }
}
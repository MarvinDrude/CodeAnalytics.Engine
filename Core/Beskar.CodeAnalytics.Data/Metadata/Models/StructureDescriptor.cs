using Beskar.CodeAnalytics.Data.Metadata.Specs;

namespace Beskar.CodeAnalytics.Data.Metadata.Models;

public sealed class StructureDescriptor
{
   /// <summary>
   /// The root folder id in folder specs.
   /// </summary>
   public required uint RootFolderId { get; set; }
   
   /// <summary>
   /// Folder spec descriptor.
   /// </summary>
   public required FolderSpecDescriptor Folders { get; set; }
   
   /// <summary>
   /// Files spec descriptor.
   /// </summary>
   public required FileSpecDescriptor Files { get; set; }
   
   /// <summary>
   /// Symbol locations spec descriptor.
   /// </summary>
   public required SymbolLocationSpecDescriptor SymbolLocations { get; set; }
   
   /// <summary>
   /// Project spec descriptor.
   /// </summary>
   public required ProjectSpecDescriptor Projects { get; set; }
   
   /// <summary>
   /// Solution spec descriptor.
   /// </summary>
   public required SolutionSpecDescriptor Solutions { get; set; }
   
   /// <summary>
   /// Syntax file spec descriptor.
   /// </summary>
   public required SyntaxFileDescriptor SyntaxFiles { get; set; }
   
   public async Task Initialize(DatabaseDescriptor database)
   {
      await Folders.Initialize(database);
   }
}
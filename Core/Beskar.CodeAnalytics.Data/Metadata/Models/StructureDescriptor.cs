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
   
   public Task Initialize(DatabaseDescriptor database)
   {
      
      return Task.CompletedTask;
   }
}
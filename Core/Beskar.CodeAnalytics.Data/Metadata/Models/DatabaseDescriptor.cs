
using Beskar.CodeAnalytics.Data.Metadata.Readers;

namespace Beskar.CodeAnalytics.Data.Metadata.Models;

/// <summary>
/// Metadata descriptor for the entire file-based database.
/// </summary>
public sealed class DatabaseDescriptor : IDisposable
{
   /// <summary>
   /// Where does all the data reside in?
   /// </summary>
   public required string BaseFolderPath { get; set; }
   
   /// <summary>
   /// All descriptors for the structure specs.
   /// </summary>
   public required StructureDescriptor Structure { get; set; }

   /// <summary>
   /// Holds all spec readers.
   /// </summary>
   internal SpecReaderCache SpecReaderCache { get; } = new ();

   public async Task Initialize(string baseFolderPath)
   {
      BaseFolderPath = baseFolderPath;
      
      await Structure.Initialize(this);
   }

   public void Dispose()
   {
      SpecReaderCache.Dispose();
   }
}
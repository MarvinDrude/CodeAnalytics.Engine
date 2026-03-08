
using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Metadata.Readers;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Strings;
using Me.Memory.Extensions;

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
   /// All descriptors for the symbols.
   /// </summary>
   public required SymbolsDescriptor Symbols { get; set; }
   
   /// <summary>
   /// Edges between symbols.
   /// </summary>
   public required SymbolEdgeSpecDescriptor Edges { get; set; }
   
   /// <summary>
   /// String pool descriptor.
   /// </summary>
   public required StringPoolDescriptor StringPool { get; set; }

   /// <summary>
   /// Holds all spec readers.
   /// </summary>
   internal SpecReaderCache SpecReaderCache { get; } = new ();

   public async Task Initialize(string baseFolderPath)
   {
      BaseFolderPath = baseFolderPath;
      
      await Edges.Initialize(this);
      await Structure.Initialize(this);
      await Symbols.Initialize(this);
      await StringPool.Initialize(this);
   }

   public static async Task<DatabaseDescriptor> Create(string baseFolderPath)
   {
      var metadataFilePath = Path.Combine(baseFolderPath, $"metadata.{FileNames.Suffix}");
      await using var metadataFile = File.Open(metadataFilePath, FileMode.Open);
      
      var descriptor = DatabaseDescriptor.DeserializeStream(metadataFile);
      await descriptor.Initialize(baseFolderPath);
      
      return descriptor;
   }

   public void Dispose()
   {
      SpecReaderCache.Dispose();
      Structure.Dispose();
      StringPool.Dispose();
   }
}
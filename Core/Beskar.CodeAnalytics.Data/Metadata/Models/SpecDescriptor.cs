using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Metadata.Readers;

namespace Beskar.CodeAnalytics.Data.Metadata.Models;

public class SpecDescriptor<TSpec>
   where TSpec : unmanaged, ISpec
{
   public required string FileName { get; set; }
   
   public virtual IComparer<TSpec> Comparer { get; } = Comparer<TSpec>.Default;
   public FileId FileId => TSpec.FileId;
   
   private DatabaseDescriptor? _database;
   private string? _fullFilePath;
   
   public virtual Task Initialize(DatabaseDescriptor database)
   {
      _database = database;
      return Task.CompletedTask;
   }

   public SpecFileReader<TSpec> GetReader()
   {
      if (_database is null) throw new InvalidOperationException("Database not initialized.");
      
      _fullFilePath ??= Path.Combine(_database.BaseFolderPath, FileName);
      return _database.SpecReaderCache.GetReader(FileId, _fullFilePath, Comparer);
   }
}
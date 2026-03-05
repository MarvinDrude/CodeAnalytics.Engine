using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Interfaces;
using Beskar.CodeAnalytics.Data.Metadata.Readers;

namespace Beskar.CodeAnalytics.Data.Metadata.Models;

public abstract class SpecDescriptor<TSpec>
   where TSpec : unmanaged, ISpec
{
   public required string FileName { get; set; }
   
   public abstract IComparer<TSpec> Comparer { get; }
   public FileId FileId => TSpec.FileId;
   
   private DatabaseDescriptor? _database;
   private string? _fullFilePath;
   
   public Task Initialize(DatabaseDescriptor database)
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
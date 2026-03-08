using Beskar.CodeAnalytics.Data.Hashing;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Strings;

public sealed class StringPoolDescriptor : IDisposable
{
   public required string FileName { get; set; }

   public StringFileReader Reader => field ??= new StringFileReader(
      _filePath ?? throw new InvalidOperationException());
   
   private string? _filePath;
   private bool _isInitialized;
   
   public Task Initialize(DatabaseDescriptor database)
   {
      _filePath = Path.Combine(database.BaseFolderPath, FileName);
      
      _isInitialized = true;
      return Task.CompletedTask;
   }

   public void Dispose()
   {
      if (!_isInitialized) return;
      Reader.Dispose();
   }
}
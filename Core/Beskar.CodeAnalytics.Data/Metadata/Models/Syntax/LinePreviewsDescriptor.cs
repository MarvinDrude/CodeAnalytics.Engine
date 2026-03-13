using Beskar.CodeAnalytics.Data.Bake.Sorting;
using Beskar.CodeAnalytics.Data.Syntax.Readers;

namespace Beskar.CodeAnalytics.Data.Metadata.Models.Syntax;

public sealed class LinePreviewsDescriptor : IDisposable
{
   public required string FileName { get; set; }
   
   public LinePreviewReader Reader => field ??= new LinePreviewReader(
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
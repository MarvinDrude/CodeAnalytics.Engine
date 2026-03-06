using Beskar.CodeAnalytics.Data.Syntax;

namespace Beskar.CodeAnalytics.Data.Metadata.Models;

public sealed class SyntaxFileDescriptor : IDisposable
{
   public required string FileName { get; set; }

   public SyntaxFileReader Reader => field ??= new SyntaxFileReader(_filePath ?? throw new InvalidOperationException());
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
      Reader?.Dispose();
   }
}
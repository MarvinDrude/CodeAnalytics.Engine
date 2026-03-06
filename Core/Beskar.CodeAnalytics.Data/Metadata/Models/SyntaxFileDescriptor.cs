namespace Beskar.CodeAnalytics.Data.Metadata.Models;

public sealed class SyntaxFileDescriptor
{
   public required string FileName { get; set; }

   public Task Initialize(DatabaseDescriptor database)
   {
      return Task.CompletedTask;
   }
}
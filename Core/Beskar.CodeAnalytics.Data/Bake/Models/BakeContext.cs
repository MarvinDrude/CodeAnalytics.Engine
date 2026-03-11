using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Storage;
using Beskar.CodeAnalytics.Data.Hashing;
using Beskar.CodeAnalytics.Data.Metadata.Builders;
using Beskar.CodeAnalytics.Data.Metadata.Storage;
using Me.Memory.Threading;

namespace Beskar.CodeAnalytics.Data.Bake.Models;

public sealed class BakeContext : IAsyncDisposable
{
   public required string OutputDirectoryPath { get; set; }
   
   public required bool DeleteIntermediateFiles { get; set; }
   
   public required StringFileWriter StringFileWriter { get; set; }
   
   public required DatabaseBuilder DatabaseBuilder { get; set; }
   
   public StringFileReader? StringFileReader { get; set; }
   
   public required WorkPool WorkPool { get; set; }
   
   public required Dictionary<FileId, string> FileNames { get; set; }

   public string GetString(StringFileView view)
   {
      return StringFileReader?.GetString(view) 
             ?? throw new InvalidOperationException();
   }
   
   public void CompleteStringWriter()
   {
      var strCount = StringFileWriter.ItemCount;
      
      StringFileWriter?.Dispose();
      StringFileWriter = null!; // fine here

      StringFileReader = new StringFileReader(Path.Combine(
         OutputDirectoryPath, Beskar.CodeAnalytics.Data.Constants.FileNames.StringPool));
      
      DatabaseBuilder.Storage.Files.Add(new StorageFileDescriptor()
      {
         FileName = Beskar.CodeAnalytics.Data.Constants.FileNames.StringPool,
         Name = "StringPool",
         Kind = StorageFileKind.StringPool,
         LastModified = DateTimeOffset.UtcNow,
         ParentName = string.Empty,
         RowCount = strCount,
         ByteCount = StringFileReader.ByteCount
      });
   }
   
   public async ValueTask DisposeAsync()
   {
      StringFileReader?.Dispose();
      StringFileWriter?.Dispose();
      
      await WorkPool.DisposeAsync();
   }
}
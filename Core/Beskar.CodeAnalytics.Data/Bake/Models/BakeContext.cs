using Beskar.CodeAnalytics.Data.Constants;
using Beskar.CodeAnalytics.Data.Hashing;
using Me.Memory.Threading;

namespace Beskar.CodeAnalytics.Data.Bake.Models;

public sealed class BakeContext : IAsyncDisposable
{
   public required string OutputDirectoryPath { get; set; }
   
   public required bool DeleteIntermediateFiles { get; set; }
   
   public required StringFileWriter StringFileWriter { get; set; }
   
   public required WorkPool WorkPool { get; set; }
   
   public required Dictionary<FileId, string> FileNames { get; set; }

   public async ValueTask DisposeAsync()
   {
      StringFileWriter.Dispose();
      await WorkPool.DisposeAsync();
   }   
}
using Beskar.CodeAnalytics.Data.Hashing;

namespace Beskar.CodeAnalytics.Data.Bake.Models;

public sealed class BakeContext : IDisposable
{
   public required string OutputDirectoryPath { get; set; }
   
   public required StringFileWriter StringFileWriter { get; set; }

   public void Dispose()
   {
      StringFileWriter.Dispose();
   }   
}
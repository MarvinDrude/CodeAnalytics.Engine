using Beskar.CodeAnalytics.FileStorage.Builders;
using Beskar.CodeAnalytics.FileStorage.Model.Metadata;

namespace Beskar.CodeAnalytics.FileStorage.Symbols.Builders;

public static class SymbolDatabaseBuilder
{
   public static DatabaseMetadata Create(string rootDirectory)
   {
      return DatabaseBuilder.Create()
         .WithName("C# Roslyn Symbol Database")
         .WithRootDirectory(rootDirectory)
         .Build();
   }
}
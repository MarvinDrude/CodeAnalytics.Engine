using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Builders;

public sealed class DatabaseBuilder
{
   public DatabaseStructureBuilder Structure { get; } = new();

   public DatabaseDescriptor Build()
   {
      return new DatabaseDescriptor()
      {
         BaseFolderPath = string.Empty,
         Structure = Structure.Build()
      };
   }
}
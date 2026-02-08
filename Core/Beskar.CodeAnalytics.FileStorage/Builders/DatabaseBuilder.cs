using Beskar.CodeAnalytics.FileStorage.Model.Metadata;

namespace Beskar.CodeAnalytics.FileStorage.Builders;

public sealed class DatabaseBuilder()
{
   private string? _name;
   private string? _rootDirectory;

   private readonly Dictionary<string, TableMetadata> _tables = [];

   public DatabaseBuilder WithName(string name)
   {
      _name = name;
      return this;
   }

   public DatabaseBuilder WithRootDirectory(string rootDirectory)
   {
      _rootDirectory = rootDirectory;
      return this;
   }

   public DatabaseMetadata Build()
   {
      ArgumentNullException.ThrowIfNull(_name);
      ArgumentNullException.ThrowIfNull(_rootDirectory);

      return new DatabaseMetadata()
      {
         Name = _name,
         RootDirectory = _rootDirectory,
         Tables = _tables,
      };
   }
   
   public static DatabaseBuilder Create() => new ();
}
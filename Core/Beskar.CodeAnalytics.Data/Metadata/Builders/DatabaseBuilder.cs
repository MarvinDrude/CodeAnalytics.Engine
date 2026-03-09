using Beskar.CodeAnalytics.Data.Metadata.Models;
using Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Strings;

namespace Beskar.CodeAnalytics.Data.Metadata.Builders;

public sealed class DatabaseBuilder
{
   public DatabaseStructureBuilder Structure { get; } = new();
   
   public DatabaseSymbolBuilder Symbols { get; } = new();
   
   public StorageBuilder Storage { get; } = new();
   
   private string? _fileNameEdgeSpec;
   private string? _stringPoolFileName;
   
   public DatabaseBuilder WithEdgeSpec(string fileName)
   {
      _fileNameEdgeSpec = fileName;
      return this;
   }

   public DatabaseBuilder WithStringPool(string fileName)
   {
      _stringPoolFileName = fileName;
      return this;
   }
   
   public DatabaseDescriptor Build()
   {
      return new DatabaseDescriptor()
      {
         BaseFolderPath = string.Empty,
         Edges = new SymbolEdgeSpecDescriptor()
         {
            FileName = _fileNameEdgeSpec
               ?? throw new InvalidOperationException("Edge spec file name is not specified")
         },
         StringPool = new StringPoolDescriptor()
         {
            FileName = _stringPoolFileName
               ?? throw new InvalidOperationException("String pool file name is not specified")
         },
         Structure = Structure.Build(),
         Symbols = Symbols.Build(),
         Storage = Storage.Build()
      };
   }
}
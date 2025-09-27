using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Models.Structure;

public sealed class DbFile
{
   public DbFileId Id { get; set; }
   
   public required string Name { get; set; }
   
   public required string RelativeFilePath { get; set; }

   public List<DbProject> Projects { get; set; } = [];
   
   public List<DbSymbolReference> SymbolReferences { get; set; } = [];
}

public readonly record struct DbFileId(long Value)
{
   public static readonly DbFileId Empty = new(0);
   
   public static implicit operator long(DbFileId id) => id.Value;
   public static implicit operator DbFileId(long value) => new(value);
}
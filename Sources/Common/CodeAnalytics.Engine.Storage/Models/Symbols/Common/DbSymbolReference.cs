using CodeAnalytics.Engine.Storage.Models.Structure;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Common;

public sealed class DbSymbolReference
{
   public DbSymbolReferenceId Id { get; set; }
   
   public required DbSymbol Symbol { get; set; }
   public required DbSymbolId SymbolId { get; set; }
   
   public required DbFile File { get; set; }
   public required DbFileId FileId { get; set; }
   
   public int SpanIndex { get; set; }
   public bool IsDefinition { get; set; }
}

public readonly record struct DbSymbolReferenceId(long Value)
{
   public static readonly DbSymbolReferenceId Empty = new(0);
   
   public static implicit operator long(DbSymbolReferenceId id) => id.Value;
   public static implicit operator DbSymbolReferenceId(long value) => new(value);
}
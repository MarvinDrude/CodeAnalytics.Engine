using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Types;

public sealed class DbEnumSymbol : DbSymbolBase<DbEnumSymbolId>
{
   public DbSymbol? UnderlyingTypeSymbol { get; set; }
   public DbSymbolId UnderlyingTypeSymbolId { get; set; }
}

public readonly record struct DbEnumSymbolId(long Value)
{
   public static readonly DbEnumSymbolId Empty = new(0);
   
   public static implicit operator long(DbEnumSymbolId id) => id.Value;
   public static implicit operator DbEnumSymbolId(long value) => new(value);
}
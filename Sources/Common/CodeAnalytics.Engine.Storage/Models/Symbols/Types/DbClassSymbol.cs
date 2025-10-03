using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Types;

public sealed class DbClassSymbol : DbTypeSymbolBase<DbClassSymbolId>
{
   public bool IsRecord { get; set; }
   
   public DbSymbolId? BaseClassSymbolId { get; set; }
   public DbSymbol? BaseClassSymbol { get; set; }
}

public readonly record struct DbClassSymbolId(long Value)
{
   public static readonly DbClassSymbolId Empty = new(0);
   
   public static implicit operator long(DbClassSymbolId id) => id.Value;
   public static implicit operator DbClassSymbolId(long value) => new(value);
}
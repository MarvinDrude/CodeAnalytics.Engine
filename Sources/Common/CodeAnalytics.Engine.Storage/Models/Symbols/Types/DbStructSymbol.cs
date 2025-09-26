using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Types;

public sealed class DbStructSymbol : DbTypeSymbolBase<DbStructSymbolId>
{
   public bool IsRecord { get; set; }
   public bool IsReadOnly { get; set; }
   public bool IsRef { get; set; }
}

public readonly record struct DbStructSymbolId(long Value)
{
   public static readonly DbStructSymbolId Empty = new(0);
   
   public static implicit operator long(DbStructSymbolId id) => id.Value;
   public static implicit operator DbStructSymbolId(long value) => new(value);
}
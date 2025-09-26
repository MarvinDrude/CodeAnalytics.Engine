using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Types;

public sealed class DbInterfaceSymbol : DbTypeSymbolBase<DbInterfaceSymbolId>
{
   public List<DbSymbol> ImplementedBy { get; set; } = [];
}

public readonly record struct DbInterfaceSymbolId(long Value)
{
   public static readonly DbInterfaceSymbolId Empty = new(0);
   
   public static implicit operator long(DbInterfaceSymbolId id) => id.Value;
   public static implicit operator DbInterfaceSymbolId(long value) => new(value);
}
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Types;

public sealed class DbInterfaceSymbol : DbTypeSymbolBase<DbInterfaceSymbolId>
{
   public List<DbClassSymbol> ImplementedByClass { get; set; } = [];
   public List<DbClassSymbol> ImplementedDirectByClass { get; set; } = [];
   
   public List<DbStructSymbol> ImplementedByStruct { get; set; } = [];
   public List<DbStructSymbol> ImplementedDirectByStruct { get; set; } = [];
   
   public List<DbInterfaceSymbol> ImplementedByInterface { get; set; } = [];
   public List<DbInterfaceSymbol> ImplementedDirectByInterface { get; set; } = [];
}

public readonly record struct DbInterfaceSymbolId(long Value)
{
   public static readonly DbInterfaceSymbolId Empty = new(0);
   
   public static implicit operator long(DbInterfaceSymbolId id) => id.Value;
   public static implicit operator DbInterfaceSymbolId(long value) => new(value);
}
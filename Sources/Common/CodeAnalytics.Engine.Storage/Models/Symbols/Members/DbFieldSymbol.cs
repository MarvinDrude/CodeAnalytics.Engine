using CodeAnalytics.Engine.Storage.Enums.Symbols;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Members;

public sealed class DbFieldSymbol : DbMemberSymbolBase<DbFieldSymbolId>
{
   public bool IsConst { get; set; }
   public bool IsReadOnly { get; set; }
   public bool IsVolatile { get; set; }
   
   public required NullAnnotation Nullability { get; set; }
   
   public DbSymbol? TypeSymbol { get; set; }
   public required DbSymbolId TypeSymbolId { get; set; }
}

public readonly record struct DbFieldSymbolId(long Value)
{
   public static readonly DbFieldSymbolId Empty = new(0);
   
   public static implicit operator long(DbFieldSymbolId id) => id.Value;
   public static implicit operator DbFieldSymbolId(long value) => new(value);
}
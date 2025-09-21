using CodeAnalytics.Engine.Storage.Enums.Symbols;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Members;

public sealed class DbPropertySymbol : DbMemberSymbolBase<DbPropertySymbolId>
{
   public required NullAnnotation Nullability { get; set; }
   
   public bool ReturnsByRefReadonly { get; set; }
   public bool ReturnsByRef { get; set; }
   
   public DbSymbol? TypeSymbol { get; set; }
   public required DbSymbolId TypeSymbolId { get; set; }
   
   public DbSymbol? GetterSymbol { get; set; }
   public DbSymbolId GetterSymbolId { get; set; }
   
   public DbSymbol? SetterSymbol { get; set; }
   public DbSymbolId SetterSymbolId { get; set; }
   
   public DbSymbol? OverriddenSymbol { get; set; }
   public DbSymbolId OverriddenSymbolId { get; set; }
}

public readonly record struct DbPropertySymbolId(long Value)
{
   public static readonly DbPropertySymbolId Empty = new(0);
   
   public static implicit operator long(DbPropertySymbolId id) => id.Value;
   public static implicit operator DbPropertySymbolId(long value) => new(value);
}
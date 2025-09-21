using CodeAnalytics.Engine.Storage.Enums.Symbols;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Members;

public sealed class DbMethodSymbol : DbMemberSymbolBase<DbMethodSymbolId>
{
   public bool IsAsync { get; set; }
   public bool IsConstructor { get; set; }
   public bool IsGeneric { get; set; }
   
   public int CyclomaticComplexity { get; set; }
   public NullAnnotation ReturnTypeNullability { get; set; } = NullAnnotation.None;
   
   public DbSymbol? ReturnTypeSymbol { get; set; }
   public DbSymbolId ReturnTypeSymbolId { get; set; }
   
   public DbSymbol? OverriddenSymbol { get; set; }
   public DbSymbolId OverriddenSymbolId { get; set; }
}

public readonly record struct DbMethodSymbolId(long Value)
{
   public static readonly DbMethodSymbolId Empty = new(0);
   
   public static implicit operator long(DbMethodSymbolId id) => id.Value;
   public static implicit operator DbMethodSymbolId(long value) => new(value);
}
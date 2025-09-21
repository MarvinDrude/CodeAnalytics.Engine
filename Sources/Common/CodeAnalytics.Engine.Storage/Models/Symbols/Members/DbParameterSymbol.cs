using CodeAnalytics.Engine.Storage.Enums.Modifiers;
using CodeAnalytics.Engine.Storage.Enums.Symbols;
using CodeAnalytics.Engine.Storage.Models.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Members;

public sealed class DbParameterSymbol : DbMemberSymbolBase<DbParameterSymbolId>
{
   public required ParameterModifier Modifiers { get; set; }
   public required NullAnnotation Nullability { get; set; }
   
   public int Ordinal { get; set; }
   
   public bool IsParams { get; set; }
   public bool IsThis { get; set; }
   public bool IsOptional { get; set; }
   public bool IsDiscard { get; set; }
   
   public bool HasExplicitDefaultValue { get; set; }
   
   public DbSymbol? TypeSymbol { get; set; }
   public DbSymbolId TypeSymbolId { get; set; }
}

public readonly record struct DbParameterSymbolId(long Value)
{
   public static readonly DbParameterSymbolId Empty = new(0);
   
   public static implicit operator long(DbParameterSymbolId id) => id.Value;
   public static implicit operator DbParameterSymbolId(long value) => new(value);
}
using CodeAnalytics.Engine.Storage.Enums.Modifiers;
using CodeAnalytics.Engine.Storage.Enums.Symbols;

namespace CodeAnalytics.Engine.Storage.Models.Symbols.Common;

public sealed class DbSymbol
{
   public DbSymbolId Id { get; set; }
   
   public required string UniqueId { get; set; }
   public required string UniqueIdHash  { get; set; }
   
   public required string Name { get; set; }
   public required string MetadataName { get; set; }
   public required string FullPathName { get; set; }
   
   public required string Language { get; set; }
   
   public bool IsVirtual { get; set; }
   public bool IsAbstract { get; set; }
   public bool IsStatic { get; set; }
   public bool IsSealed { get; set; }
   public bool IsGenerated { get; set; }
   
   public AccessModifier AccessModifier { get; set; } = AccessModifier.NotApplicable;
   public required SymbolType Type { get; set; }
   
   public DateTimeOffset CreatedAt { get; set; }
   
   public List<DbSymbolReference> References { get; set; }
}

public readonly record struct DbSymbolId(long Value)
{
   public static readonly DbSymbolId Empty = new(0);
   
   public static implicit operator long(DbSymbolId id) => id.Value;
   public static implicit operator DbSymbolId(long value) => new(value);
}
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Common;

public abstract class DbSymbolBase
{
   [ForeignKey(nameof(SymbolId))]
   public DbSymbol? Symbol { get; set; }
   public required long SymbolId { get; set; }
}
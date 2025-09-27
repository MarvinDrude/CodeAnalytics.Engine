namespace CodeAnalytics.Engine.Storage.Models.Symbols.Common;

public abstract class DbSymbolBase<TDbIdentifier>
   where TDbIdentifier : struct
{
   public TDbIdentifier Id { get; set; }
   
   public DbSymbol? Symbol { get; set; }
   public required DbSymbolId SymbolId { get; set; }
}
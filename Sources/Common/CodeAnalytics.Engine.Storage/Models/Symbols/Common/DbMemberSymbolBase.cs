
namespace CodeAnalytics.Engine.Storage.Models.Symbols.Common;

public abstract class DbMemberSymbolBase<TDbIdentifier>
   : DbSymbolBase<TDbIdentifier>
   where TDbIdentifier : struct
{
   public DbSymbol? ContainingSymbol { get; set; }
   public required DbSymbolId ContainingSymbolId { get; set; }
}
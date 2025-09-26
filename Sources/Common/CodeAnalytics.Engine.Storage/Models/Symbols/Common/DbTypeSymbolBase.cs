namespace CodeAnalytics.Engine.Storage.Models.Symbols.Common;

public abstract class DbTypeSymbolBase<TDbIdentifier>
   : DbSymbolBase<TDbIdentifier>
   where TDbIdentifier : struct
{
   public bool IsAnonymous { get; set; }
   
   
}
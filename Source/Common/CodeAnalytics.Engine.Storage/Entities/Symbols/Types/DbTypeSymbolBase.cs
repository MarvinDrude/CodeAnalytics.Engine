using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Types;

public abstract class DbTypeSymbolBase : DbSymbolBase
{
   public bool IsAnonymous { get; set; }
}
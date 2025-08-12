using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Types;

[Index(nameof(Id), IsUnique = true)]
public sealed class DbInterfaceSymbol : DbTypeSymbolBase
{
   public long Id { get; set; }   
}
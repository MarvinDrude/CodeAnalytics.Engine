using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Types;

[Index(nameof(Id), IsUnique = true)]
public sealed class DbStructSymbol : DbTypeSymbolBase
{
   public long Id { get; set; }
   
   public bool IsRecord { get; set; }
   public bool IsReadOnly { get; set; }
   public bool IsRef { get; set; }
}
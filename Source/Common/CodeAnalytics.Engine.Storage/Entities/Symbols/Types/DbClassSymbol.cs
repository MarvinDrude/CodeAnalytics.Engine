using System.ComponentModel.DataAnnotations.Schema;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Types;

[Index(nameof(Id), IsUnique = true)]
public sealed class DbClassSymbol : DbTypeSymbolBase
{
   public long Id { get; set; }
   
   public bool IsRecord { get; set; }
   
   [ForeignKey(nameof(BaseClassSymbol))]
   public long BaseClassSymbolId { get; set; }
   public DbSymbol? BaseClassSymbol { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Types;

[Index(nameof(Id), IsUnique = true)]
public sealed class DbEnumSymbol : DbSymbolBase
{
   public long Id { get; set; }
   
   [ForeignKey(nameof(UnderlyingTypeId))]
   public DbSymbol? UnderlyingType { get; set; }
   public long UnderlyingTypeId { get; set; }
}
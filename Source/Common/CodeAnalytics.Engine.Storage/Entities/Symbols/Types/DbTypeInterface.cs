using System.ComponentModel.DataAnnotations.Schema;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Types;

[PrimaryKey(nameof(TypeSymbol), nameof(InterfaceSymbolId))]
[Index(nameof(TypeSymbol))]
[Index(nameof(InterfaceSymbolId))]
public sealed class DbTypeInterface
{
   [ForeignKey(nameof(TypeSymbolId))]
   public DbSymbol? TypeSymbol { get; set; }
   public required long TypeSymbolId { get; set; }
   
   [ForeignKey(nameof(InterfaceSymbolId))]
   public DbSymbol? InterfaceSymbol { get; set; }
   public required long InterfaceSymbolId { get; set; }
   
   public bool IsDirect { get; set; }
}
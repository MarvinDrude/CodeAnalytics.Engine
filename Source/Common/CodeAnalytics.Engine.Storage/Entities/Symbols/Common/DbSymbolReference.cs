using System.ComponentModel.DataAnnotations.Schema;
using CodeAnalytics.Engine.Storage.Entities.Structure;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Common;

[Index(nameof(Id), IsUnique = true)]
public sealed class DbSymbolReference
{
   public long Id { get; set; }
   
   [ForeignKey(nameof(SymbolId))]
   public DbSymbol? Symbol { get; set; }
   public required long SymbolId { get; set; }
   
   [ForeignKey(nameof(FileId))]
   public DbFile? File { get; set; }
   public required long FileId { get; set; }
   
   public int SpanIndex { get; set; }
   public bool IsDefinition { get; set; }
}
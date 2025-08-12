using System.ComponentModel.DataAnnotations.Schema;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Members;

public abstract class DbMemberSymbolBase : DbSymbolBase
{
   [ForeignKey(nameof(ContainingSymbolId))]
   public DbSymbol? ContainingSymbol { get; set; }
   public required long ContainingSymbolId { get; set; }
}
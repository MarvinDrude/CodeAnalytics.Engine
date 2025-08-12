using System.ComponentModel.DataAnnotations.Schema;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Members;

[PrimaryKey(nameof(SymbolId), nameof(InterfaceSymbolId))]
[Index(nameof(SymbolId))]
[Index(nameof(InterfaceSymbolId))]
public sealed class DbMemberInterfaceImplementation
{
   [ForeignKey(nameof(Symbol))]
   public required long SymbolId { get; set; }
   public DbSymbol? Symbol { get; set; }
   
   [ForeignKey(nameof(InterfaceSymbol))]
   public required long InterfaceSymbolId { get; set; }
   public DbSymbol? InterfaceSymbol { get; set; }
   
   public bool IsExplicit { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using CodeAnalytics.Engine.Storage.Enums.Symbols;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Members;

[Index(nameof(Id), IsUnique = true)]
public sealed class DbMethodSymbol
{
   public long Id { get; set; }
   
   public bool IsAsync { get; set; }
   public bool IsConstructor { get; set; }
   public bool IsGeneric { get; set; }
   
   public int CyclomaticComplexity { get; set; }
   public NullAnnotation ReturnTypeNullability { get; set; } = NullAnnotation.None;
   
   [ForeignKey(nameof(ReturnTypeId))]
   public DbSymbol? ReturnType { get; set; }
   public long ReturnTypeId { get; set; }
   
   [ForeignKey(nameof(OverriddenSymbolId))]
   public DbSymbol? OverriddenSymbol { get; set; }
   public long OverriddenSymbolId { get; set; }
}
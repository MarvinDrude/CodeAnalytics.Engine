using System.ComponentModel.DataAnnotations.Schema;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using CodeAnalytics.Engine.Storage.Enums.Symbols;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Members;

[Index(nameof(Id), IsUnique = true)]
public sealed class DbPropertySymbol : DbMemberSymbolBase
{
   public long Id { get; set; }
   
   public required NullAnnotation Nullability { get; set; }
   
   public bool ReturnsByRefReadonly { get; set; }
   public bool ReturnsByRef { get; set; }
   
   public int GetterCyclomaticComplexity { get; set; }
   public int SetterCylclomaticComplexity { get; set; }
   
   [ForeignKey(nameof(TypeSymbolId))]
   public DbSymbol? TypeSymbol { get; set; }
   public required long TypeSymbolId { get; set; }
   
   [ForeignKey(nameof(GetterMethodId))]
   public DbSymbol? GetterMethod { get; set; }
   public long GetterMethodId { get; set; }
   
   [ForeignKey(nameof(SetterMethodId))]
   public DbSymbol? SetterMethod { get; set; }
   public long SetterMethodId { get; set; }
   
   [ForeignKey(nameof(OverriddenSymbolId))]
   public DbSymbol? OverriddenSymbol { get; set; }
   public long OverriddenSymbolId { get; set; }
}
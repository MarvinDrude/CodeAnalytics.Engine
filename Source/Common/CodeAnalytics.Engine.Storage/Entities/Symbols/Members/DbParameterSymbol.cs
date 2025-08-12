using System.ComponentModel.DataAnnotations.Schema;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using CodeAnalytics.Engine.Storage.Enums.Modifiers;
using CodeAnalytics.Engine.Storage.Enums.Symbols;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Members;

[Index(nameof(Id), IsUnique = true)]
public class DbParameterSymbol : DbMemberSymbolBase
{
   public long Id { get; set; }
   
   public required ParameterModifier Modifiers { get; set; }
   public required NullAnnotation Nullability { get; set; }
   
   public int Ordinal { get; set; }
   
   public bool IsParams { get; set; }
   public bool IsThis { get; set; }
   public bool IsOptional { get; set; }
   public bool IsDiscard { get; set; }
   
   public bool HasExplicitDefaultValue { get; set; }
   
   [ForeignKey(nameof(TypeSymbolId))]
   public DbSymbol? TypeSymbol { get; set; }
   public required long TypeSymbolId { get; set; }
}
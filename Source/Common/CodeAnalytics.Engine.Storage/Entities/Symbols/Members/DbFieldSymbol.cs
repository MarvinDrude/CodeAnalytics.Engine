using System.ComponentModel.DataAnnotations.Schema;
using CodeAnalytics.Engine.Storage.Entities.Symbols.Common;
using CodeAnalytics.Engine.Storage.Enums.Symbols;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Entities.Symbols.Members;

[Index(nameof(Id), IsUnique = true)]
public sealed class DbFieldSymbol : DbMemberSymbolBase
{
   public long Id { get; set; }
   
   public bool IsConst { get; set; }
   public bool IsReadOnly { get; set; }
   public bool IsVolatile { get; set; }
   
   public required NullAnnotation Nullability { get; set; }
   
   [ForeignKey(nameof(TypeSymbolId))]
   public DbSymbol? TypeSymbol { get; set; }
   public required long TypeSymbolId { get; set; }
}
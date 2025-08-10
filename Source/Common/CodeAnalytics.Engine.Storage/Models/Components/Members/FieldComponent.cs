using CodeAnalytics.Engine.Storage.Models.Components.Common;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Members;

[Index(nameof(Id), IsUnique = true)]
public sealed class FieldComponent : MemberComponent
{
   public long Id { get; set; }
   
   public bool IsReadOnly { get; set; }
   public bool IsConst { get; set; }
   
   public required long TypeId { get; set; }
   public required SymbolComponent Type { get; set; }
}
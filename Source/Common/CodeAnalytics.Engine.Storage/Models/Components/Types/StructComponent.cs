using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Types;

[Index(nameof(Id), IsUnique = true)]
public sealed class StructComponent : TypeComponent
{
   public long Id { get; set; }
   
   public bool IsRef { get; set; }
   public bool IsReadOnly { get; set; }
}
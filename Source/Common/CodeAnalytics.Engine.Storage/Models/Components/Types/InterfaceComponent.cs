using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Types;

[Index(nameof(Id), IsUnique = true)]
public class InterfaceComponent : TypeComponent
{
   public long Id { get; set; }
}
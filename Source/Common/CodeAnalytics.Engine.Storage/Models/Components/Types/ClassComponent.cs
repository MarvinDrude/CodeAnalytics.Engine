using CodeAnalytics.Engine.Storage.Models.Components.Members;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Types;

[Index(nameof(Id), IsUnique = true)]
public sealed class ClassComponent : TypeComponent
{
   public long Id { get; set; }
   
   public bool IsStatic { get; set; }
   public bool IsAbstract { get; set; }
   public bool IsSealed { get; set; }
   
   public long BaseClassId { get; set; }
   public ClassComponent? BaseClass { get; set; }
   public ClassComponent? DerivedClass { get; set; }
}
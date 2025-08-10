using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Members;

[Index(nameof(Id), IsUnique = true)]
public sealed class PropertyComponent : MemberComponent
{
   public long Id { get; set; }
   
   public bool HasGetter { get; set; }
   public bool HasSetter { get; set; }
   
   public int GetterCyclomaticComplexity { get; set; }
   public int SetterCyclomaticComplexity { get; set; }
   
   public List<PropertyComponent> InterfaceImplementations { get; set; } = [];
   
   public long OverriddenPropertyId { get; set; }
   public PropertyComponent? OverriddenProperty { get; set; }
}
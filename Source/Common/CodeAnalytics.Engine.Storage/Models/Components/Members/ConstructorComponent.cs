using CodeAnalytics.Engine.Storage.Models.Components.Common;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Members;

[Index(nameof(Id), IsUnique = true)]
public sealed class ConstructorComponent : MemberComponent
{
   public required long Id { get; set; }
   
   public int CyclomaticComplexity { get; set; }
   
   public List<ParameterComponent> ParameterComponents { get; set; } = [];
}
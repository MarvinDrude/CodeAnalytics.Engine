using CodeAnalytics.Engine.Storage.Models.Components.Common;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Members;

[Index(nameof(Id), IsUnique = true)]
public sealed class MethodComponent : MemberComponent
{
   public long Id { get; set; }
   
   public bool IsAbstract { get; set; }
   public bool IsOverride { get; set; }
   
   public int CyclomaticComplexity { get; set; }
   
   public List<ParameterComponent> ParameterComponents { get; set; } = [];
   public List<MethodComponent> InterfaceImplementations { get; set; } = [];
   
   public long OverriddenMethodId { get; set; }
   public MethodComponent? OverriddenMethod { get; set; }
   
   public required long TypeId { get; set; }
   public required SymbolComponent Type { get; set; }
}
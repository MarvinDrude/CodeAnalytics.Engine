using CodeAnalytics.Engine.Storage.Enums.Modifiers;
using CodeAnalytics.Engine.Storage.Models.Components.Interfaces;
using CodeAnalytics.Engine.Storage.Models.Components.Members;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Common;

[Index(nameof(Id), IsUnique = true)]
public sealed class ParameterComponent : IComponent
{
   public long Id { get; set; }

   public ParameterModifier Modifier { get; set; } = ParameterModifier.None;
   
   public required long TypeId { get; set; }
   public required SymbolComponent Type { get; set; }
   
   public long ConstructorComponentId { get; set; }
   public ConstructorComponent? ConstructorComponent { get; set; }
   
   public long MethodComponentId { get; set; }
   public MethodComponent? MethodComponent { get; set; }
   
   public required long SymbolComponentId { get; set; }
   public required SymbolComponent SymbolComponent { get; set; }
}
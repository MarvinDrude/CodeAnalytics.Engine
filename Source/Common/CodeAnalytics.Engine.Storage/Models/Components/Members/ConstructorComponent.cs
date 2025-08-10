using CodeAnalytics.Engine.Storage.Models.Components.Common;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Members;

[Index(nameof(Id), IsUnique = true)]
public sealed class ConstructorComponent
{
   public required long Id { get; set; }
   
   public int CyclomaticComplexity { get; set; }
   
   public List<ParameterComponent> Parameters { get; set; } = [];
   
   public required long SymbolComponentId { get; set; }
   public required SymbolComponent SymbolComponent { get; set; }
}
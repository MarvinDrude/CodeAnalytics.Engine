using CodeAnalytics.Engine.Storage.Models.Components.Common;
using CodeAnalytics.Engine.Storage.Models.Components.Interfaces;
using CodeAnalytics.Engine.Storage.Models.Components.Members;
using Microsoft.EntityFrameworkCore;

namespace CodeAnalytics.Engine.Storage.Models.Components.Types;

[Index(nameof(Id), IsUnique = true)]
public sealed class EnumComponent : IComponent
{
   public long Id { get; set; }
   
   public required long SymbolComponentId { get; set; }
   public required SymbolComponent SymbolComponent { get; set; }
}
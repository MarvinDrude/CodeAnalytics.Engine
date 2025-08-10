using CodeAnalytics.Engine.Storage.Enums.Modifiers;
using CodeAnalytics.Engine.Storage.Models.Components.Common;

namespace CodeAnalytics.Engine.Storage.Models.Components.Members;

public abstract class MemberComponent
{
   public required AccessModifier Access { get; set; }
   public bool IsStatic { get; set; }
   
   public required long SymbolComponentId { get; set; }
   public required SymbolComponent SymbolComponent { get; set; }
   
   public required long ContainingTypeId { get; set; }
   public required SymbolComponent ContainingType { get; set; }
}
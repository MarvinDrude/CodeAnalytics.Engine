
using CodeAnalytics.Engine.Storage.Models.Components.Common;
using CodeAnalytics.Engine.Storage.Models.Components.Interfaces;
using CodeAnalytics.Engine.Storage.Models.Components.Members;

namespace CodeAnalytics.Engine.Storage.Models.Components.Types;

public abstract class TypeComponent : IComponent
{
   public List<InterfaceComponent> Interfaces { get; set; } = [];
   public List<InterfaceComponent> DirectInterfaces { get; set; } = [];
   
   public List<ConstructorComponent> ConstructorComponents { get; set; } = [];
   public List<MethodComponent> MethodComponents { get; set; } = [];
   public List<PropertyComponent> PropertyComponents { get; set; } = [];
   public List<FieldComponent> FieldComponents { get; set; } = [];
   
   public required long SymbolComponentId { get; set; }
   public required SymbolComponent SymbolComponent { get; set; }
}
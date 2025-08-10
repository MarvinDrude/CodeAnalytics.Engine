using CodeAnalytics.Engine.Storage.Models.Components.Common;

namespace CodeAnalytics.Engine.Storage.Models.Components.Interfaces;

public interface IComponent
{
   public long SymbolComponentId { get; set; }
   public SymbolComponent SymbolComponent { get; set; }
}
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Inerfaces;

public interface IComponent
{
   public NodeId NodeId { get; }
}
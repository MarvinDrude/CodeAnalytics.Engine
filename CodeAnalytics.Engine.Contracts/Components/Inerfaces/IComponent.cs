using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Components.Inerfaces;

public interface IComponent : IDisposable
{
   public NodeId NodeId { get; }
}
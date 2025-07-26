using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;

public interface IArchetype : IDisposable
{
   public NodeId NodeId { get; }
   
   public SymbolComponent SymbolComponent { get; }
}
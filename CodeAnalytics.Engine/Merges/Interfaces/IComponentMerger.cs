using CodeAnalytics.Engine.Contracts.Components.Inerfaces;

namespace CodeAnalytics.Engine.Merges.Interfaces;

public interface IComponentMerger<TComponent>
   where TComponent : IComponent
{
   public static abstract void Merge(ref TComponent target, ref TComponent source);
}
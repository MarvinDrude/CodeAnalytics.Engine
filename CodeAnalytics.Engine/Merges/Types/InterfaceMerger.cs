using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Types;

public sealed class InterfaceMerger : IComponentMerger<InterfaceComponent>
{
   public static void Merge(ref InterfaceComponent target, ref InterfaceComponent source)
   {
      // none to merge yet
   }
}
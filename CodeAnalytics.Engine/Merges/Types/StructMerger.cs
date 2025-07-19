using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Types;

public sealed class StructMerger : IComponentMerger<StructComponent>
{
   public static void Merge(ref StructComponent target, ref StructComponent source)
   {
      target.IsRef = target.IsRef || source.IsRef;
      target.IsReadOnly = target.IsReadOnly || source.IsReadOnly;
   }
}
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Members;

public sealed class ConstructorMerger : IComponentMerger<ConstructorComponent>
{
   public static void Merge(ref ConstructorComponent target, ref ConstructorComponent source)
   {
      target.ParameterIds.AddRange(source.ParameterIds);
   }
}
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Members;

public sealed class ParameterMerger : IComponentMerger<ParameterComponent>
{
   public static void Merge(ref ParameterComponent target, ref ParameterComponent source)
   {
      target.AttributeIds.AddRange(source.AttributeIds);
      target.Modifiers |= source.Modifiers;
   }
}
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Members;

public sealed class MethodMerger : IComponentMerger<MethodComponent>
{
   public static void Merge(ref MethodComponent target, ref MethodComponent source)
   {
      target.CyclomaticComplexity = Math.Max(target.CyclomaticComplexity, source.CyclomaticComplexity);
      
      target.IsAbstract = target.IsAbstract || source.IsAbstract;
      target.IsAsync = target.IsAsync || source.IsAsync;
      target.IsOverride = target.IsOverride || source.IsOverride;
   }
}
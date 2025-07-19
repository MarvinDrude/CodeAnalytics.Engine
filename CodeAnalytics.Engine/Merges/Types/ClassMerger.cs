using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Types;

public sealed class ClassMerger : IComponentMerger<ClassComponent>
{
   public static void Merge(ref ClassComponent target, ref ClassComponent source)
   {
      target.IsAbstract = target.IsAbstract || source.IsAbstract;
      target.IsSealed = target.IsSealed || source.IsSealed;
      target.IsStatic = target.IsStatic || source.IsStatic;

      if (target.BaseClassId.IsEmpty)
      {
         target.BaseClassId = source.BaseClassId;
      }
   }
}
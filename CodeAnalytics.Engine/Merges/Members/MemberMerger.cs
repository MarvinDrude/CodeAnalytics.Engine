using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Members;

public sealed class MemberMerger : IComponentMerger<MemberComponent>
{
   public static void Merge(ref MemberComponent target, ref MemberComponent source)
   {
      target.AttributeIds.AddRange(source.AttributeIds);
      target.IsStatic = target.IsStatic || source.IsStatic;

      if (target.ContainingTypeId.IsEmpty)
      {
         target.ContainingTypeId = source.ContainingTypeId;
      }

      if (target.MemberTypeId.IsEmpty)
      {
         target.MemberTypeId = source.MemberTypeId;
      }
   }
}
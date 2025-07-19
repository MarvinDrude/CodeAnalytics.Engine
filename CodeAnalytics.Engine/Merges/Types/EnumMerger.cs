using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Types;

public sealed class EnumMerger : IComponentMerger<EnumComponent>
{
   public static void Merge(ref EnumComponent target, ref EnumComponent source)
   {
      if (target.UnderlyingTypeId.IsEmpty)
      {
         target.UnderlyingTypeId = source.UnderlyingTypeId;
      }
      
      target.ValueIds.AddRange(source.ValueIds);
   }
}
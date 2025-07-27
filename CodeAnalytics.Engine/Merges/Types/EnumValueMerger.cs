using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Types;

public sealed class EnumValueMerger : IComponentMerger<EnumValueComponent>
{
   public static void Merge(ref EnumValueComponent target, ref EnumValueComponent source)
   {
      // nothing to merge
   }
}
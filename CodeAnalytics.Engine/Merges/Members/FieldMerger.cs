using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Members;

public sealed class FieldMerger : IComponentMerger<FieldComponent>
{
   public static void Merge(ref FieldComponent target, ref FieldComponent source)
   {
      target.IsReadOnly = target.IsReadOnly || source.IsReadOnly;
      target.IsConst = target.IsConst || source.IsConst;
   }
}
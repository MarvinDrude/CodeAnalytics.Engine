using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Members;

public sealed class PropertyMerger : IComponentMerger<PropertyComponent>
{
   public static void Merge(ref PropertyComponent target, ref PropertyComponent source)
   {
      target.HasGetter = target.HasGetter || source.HasGetter;
      target.HasSetter = target.HasSetter || source.HasSetter;
      
      target.InterfaceImplementations.AddSpanRange(source.InterfaceImplementations.WrittenSpan);

      if (target.OverrideId.IsEmpty)
      {
         target.OverrideId = source.OverrideId;
      }
   }
}
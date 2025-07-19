using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Types;

public sealed class TypeMerger : IComponentMerger<TypeComponent>
{
   public static void Merge(ref TypeComponent target, ref TypeComponent source)
   {
      target.InterfaceIds.AddRange(source.InterfaceIds);
      target.DirectInterfaceIds.AddRange(source.DirectInterfaceIds);
      
      target.ConstructorIds.AddRange(source.ConstructorIds);
      target.FieldIds.AddRange(source.FieldIds);
      target.MethodIds.AddRange(source.MethodIds);
      target.PropertyIds.AddRange(source.PropertyIds);
      
      target.AttributeIds.AddRange(source.AttributeIds);
   }
}
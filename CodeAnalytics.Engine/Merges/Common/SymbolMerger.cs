using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Merges.Interfaces;

namespace CodeAnalytics.Engine.Merges.Common;

public sealed class SymbolMerger : IComponentMerger<SymbolComponent>
{
   public static void Merge(ref SymbolComponent target, ref SymbolComponent source)
   {
      target.FileLocations.AddRange(source.FileLocations);
      target.Projects.AddRange(source.Projects);
      target.Declarations.AddRange(source.Declarations);
   }
}
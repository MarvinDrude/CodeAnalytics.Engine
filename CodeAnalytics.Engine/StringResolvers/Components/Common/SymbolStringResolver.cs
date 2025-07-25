using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.StringResolvers;
using CodeAnalytics.Engine.Extensions.Collections;

namespace CodeAnalytics.Engine.StringResolvers.Components.Common;

public sealed class SymbolStringResolver : IStringResolver<SymbolComponent>
{
   public ValueTask Resolve(Dictionary<int, string> result, ref SymbolComponent input)
   {
      input.Name.Resolve(result);
      input.MetadataName.Resolve(result);
      input.FullPathName.Resolve(result);

      input.FileLocations.Resolve(result);
      input.Projects.Resolve(result);
      
      return ValueTask.CompletedTask;
   }
}
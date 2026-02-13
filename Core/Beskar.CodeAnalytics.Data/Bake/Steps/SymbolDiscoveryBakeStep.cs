using Beskar.CodeAnalytics.Data.Bake.Interfaces;
using Beskar.CodeAnalytics.Data.Bake.Models;

namespace Beskar.CodeAnalytics.Data.Bake.Steps;

public sealed class SymbolDiscoveryBakeStep : IBakeStep
{
   public string Name => "Symbol Discovery";
   
   public ValueTask Execute(BakeContext context, CancellationToken cancellationToken = default)
   {
      return ValueTask.CompletedTask;
   }
}
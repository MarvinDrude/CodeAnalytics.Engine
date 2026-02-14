using Beskar.CodeAnalytics.Data.Bake.Interfaces;
using Beskar.CodeAnalytics.Data.Bake.Models;

namespace Beskar.CodeAnalytics.Data.Bake.Steps;

public sealed class EdgeConnectionBakeStep : IBakeStep
{
   public string Name => "Edge Connections";
   
   public ValueTask Execute(BakeContext context, CancellationToken cancellationToken = default)
   {
      throw new NotImplementedException();
   }
}
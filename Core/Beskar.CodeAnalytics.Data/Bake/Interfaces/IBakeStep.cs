using Beskar.CodeAnalytics.Data.Bake.Models;

namespace Beskar.CodeAnalytics.Data.Bake.Interfaces;

public interface IBakeStep
{
   public string Name { get; }

   public ValueTask Execute(BakeContext context, CancellationToken cancellationToken = default);
}
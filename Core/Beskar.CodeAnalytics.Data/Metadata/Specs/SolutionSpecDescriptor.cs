using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs;

public sealed class SolutionSpecDescriptor
   : SpecDescriptor<SolutionSpec>
{
   public override IComparer<SolutionSpec> Comparer => field ??= Comparer<SolutionSpec>.Create(
      static (x, y) => x.Id.CompareTo(y.Id));
}
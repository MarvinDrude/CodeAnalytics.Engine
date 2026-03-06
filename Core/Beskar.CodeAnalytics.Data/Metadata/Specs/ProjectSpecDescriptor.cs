using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs;

public class ProjectSpecDescriptor
   : SpecDescriptor<ProjectSpec>
{
   public override IComparer<ProjectSpec> Comparer => field ??= Comparer<ProjectSpec>.Create(
      static (x, y) => x.Id.CompareTo(y.Id));
}
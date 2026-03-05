using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs;

public sealed class FolderSpecDescriptor
   : SpecDescriptor<FolderSpec>
{
   public override IComparer<FolderSpec> Comparer => field ??= Comparer<FolderSpec>.Create(
      static (x, y) => x.Id.CompareTo(y.Id));
}
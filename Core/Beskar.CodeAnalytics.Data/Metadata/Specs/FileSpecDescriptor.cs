using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs;

public class FileSpecDescriptor
   : SpecDescriptor<FileSpec>
{
   public override IComparer<FileSpec> Comparer => field ??= Comparer<FileSpec>.Create(
      static (x, y) => x.Id.CompareTo(y.Id));
}
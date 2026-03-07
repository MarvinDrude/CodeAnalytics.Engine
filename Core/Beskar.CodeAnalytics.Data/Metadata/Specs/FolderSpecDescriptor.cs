using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Metadata.Indexes;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs;

public sealed class FolderSpecDescriptor
   : SpecDescriptor<FolderSpec>
{
   public required Indexes Index { get; set; }
   
   public override IComparer<FolderSpec> Comparer => field ??= Comparer<FolderSpec>.Create(
      static (x, y) => x.Id.CompareTo(y.Id));

   public override async Task Initialize(DatabaseDescriptor database)
   {
      await base.Initialize(database);
      
      Index.ParentId.Initialize(database);
   }

   public sealed class Indexes
   {
      public required BTreeIndexDescriptor<uint> ParentId { get; init; }
   }
}
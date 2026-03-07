using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Indexes;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class SymbolSpecDescriptor
   : SpecDescriptor<SymbolSpec>
{
   public required Indexes Index { get; set; }
   
   public override IComparer<SymbolSpec> Comparer => field ??= Comparer<SymbolSpec>.Create(
      static (x, y) => x.Id.CompareTo(y.Id));

   public override async Task Initialize(DatabaseDescriptor database)
   {
      await base.Initialize(database);
      
      Index.FullPathName.Initialize(database);
   }
   
   public sealed class Indexes
   {
      public required NGramIndexDescriptor FullPathName { get; init; }
   }
}
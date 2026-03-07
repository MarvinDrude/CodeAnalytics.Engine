using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Indexes;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class MethodSymbolSpecDescriptor
   : SpecDescriptor<MethodSymbolSpec>
{
   public required Indexes Index { get; set; }
   
   public override IComparer<MethodSymbolSpec> Comparer => field ??= Comparer<MethodSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
   
   public override async Task Initialize(DatabaseDescriptor database)
   {
      await base.Initialize(database);
      Index.ParameterCount.Initialize(database);
   }

   public sealed class Indexes
   {
      public required BTreeIndexDescriptor<int> ParameterCount { get; set; }
   }
}
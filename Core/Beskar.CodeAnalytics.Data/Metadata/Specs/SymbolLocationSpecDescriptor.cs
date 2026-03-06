using Beskar.CodeAnalytics.Data.Entities.Structure;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs;

public sealed class SymbolLocationSpecDescriptor
   : SpecDescriptor<SymbolLocationSpec>
{
   public override IComparer<SymbolLocationSpec> Comparer => field ??= SymbolLocationSpec.Comparer;
}
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class NamedTypeSymbolSpecDescriptor
   : SpecDescriptor<NamedTypeSymbolSpec>
{
   public override IComparer<NamedTypeSymbolSpec> Comparer => field ??= Comparer<NamedTypeSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
}
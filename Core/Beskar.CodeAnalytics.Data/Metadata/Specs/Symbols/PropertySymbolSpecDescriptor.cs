using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class PropertySymbolSpecDescriptor
   : SpecDescriptor<PropertySymbolSpec>
{
   public override IComparer<PropertySymbolSpec> Comparer => field ??= Comparer<PropertySymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
}
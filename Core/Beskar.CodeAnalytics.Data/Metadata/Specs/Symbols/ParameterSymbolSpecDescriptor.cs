using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class ParameterSymbolSpecDescriptor
   : SpecDescriptor<ParameterSymbolSpec>
{
   public override IComparer<ParameterSymbolSpec> Comparer => field ??= Comparer<ParameterSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
}
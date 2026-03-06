using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class SymbolSpecDescriptor
   : SpecDescriptor<SymbolSpec>
{
   public override IComparer<SymbolSpec> Comparer => field ??= Comparer<SymbolSpec>.Create(
      static (x, y) => x.Id.CompareTo(y.Id));
}
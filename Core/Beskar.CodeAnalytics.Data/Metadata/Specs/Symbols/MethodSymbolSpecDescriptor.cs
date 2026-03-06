using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class MethodSymbolSpecDescriptor
   : SpecDescriptor<MethodSymbolSpec>
{
   public override IComparer<MethodSymbolSpec> Comparer => field ??= Comparer<MethodSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
}
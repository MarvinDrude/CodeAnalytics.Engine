using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class TypeSymbolSpecDescriptor
   : SpecDescriptor<TypeSymbolSpec>
{
   public override IComparer<TypeSymbolSpec> Comparer => field ??= Comparer<TypeSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
}
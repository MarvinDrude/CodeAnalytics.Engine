using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class TypeParameterSymbolSpecDescriptor
   : SpecDescriptor<TypeParameterSymbolSpec>
{
   public override IComparer<TypeParameterSymbolSpec> Comparer => field ??= Comparer<TypeParameterSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
}
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class FieldSymbolSpecDescriptor
   : SpecDescriptor<FieldSymbolSpec>
{
   public override IComparer<FieldSymbolSpec> Comparer => field ??= Comparer<FieldSymbolSpec>.Create(
      static (x, y) => x.SymbolId.CompareTo(y.SymbolId));
}
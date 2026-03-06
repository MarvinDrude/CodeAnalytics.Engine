using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Metadata.Models;

namespace Beskar.CodeAnalytics.Data.Metadata.Specs.Symbols;

public sealed class SymbolEdgeSpecDescriptor
   : SpecDescriptor<SymbolEdgeSpec>
{
   public override IComparer<SymbolEdgeSpec> Comparer => field ??= Comparer<SymbolEdgeSpec>.Create(
      static (x, y) =>
      {
         var compareSource = x.SourceSymbolId.CompareTo(y.SourceSymbolId);
         if (compareSource != 0) return compareSource;
         
         var compareType = x.Type.CompareTo(y.Type);
         if (compareType != 0) return compareType;
         
         return x.TargetSymbolId.CompareTo(y.TargetSymbolId);
      });
}
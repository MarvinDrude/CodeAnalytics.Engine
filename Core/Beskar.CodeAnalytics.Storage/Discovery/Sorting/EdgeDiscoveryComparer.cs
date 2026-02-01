using Beskar.CodeAnalytics.Storage.Entities.Edges;

namespace Beskar.CodeAnalytics.Storage.Discovery.Sorting;

public sealed class EdgeDiscoveryComparer : IComparer<EdgeDefinition>
{
   public int Compare(EdgeDefinition x, EdgeDefinition y)
   {
      var primary = x.Key.SourceSymbolId.CompareTo(y.Key.SourceSymbolId);
      if (primary != 0) return primary;

      var secondary = x.Key.EdgeType.CompareTo(y.Key.EdgeType);
      if (secondary != 0) return secondary;
      
      var last = x.Key.SourceSymbolId.CompareTo(y.Key.SourceSymbolId);
      return last;
   }
}
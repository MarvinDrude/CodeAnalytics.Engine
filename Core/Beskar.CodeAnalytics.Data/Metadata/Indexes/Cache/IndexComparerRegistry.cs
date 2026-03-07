namespace Beskar.CodeAnalytics.Data.Metadata.Indexes.Cache;

public static class IndexComparerRegistry<T>
{
   private static readonly Dictionary<string, IComparer<T>> _idToComparer = [];
   private static readonly Dictionary<IComparer<T>, string> _typeToId = new (ReferenceEqualityComparer.Instance);
   
   public static void Register<TComparer>(string name, TComparer comparer)
      where TComparer : IComparer<T>
   {
      _typeToId[comparer] = name;
      _idToComparer[name] = comparer;
   }
   
   public static string? GetId(IComparer<T> comparer) => _typeToId.GetValueOrDefault(comparer);
   public static IComparer<T> GetComparer(string id) => _idToComparer[id];
}
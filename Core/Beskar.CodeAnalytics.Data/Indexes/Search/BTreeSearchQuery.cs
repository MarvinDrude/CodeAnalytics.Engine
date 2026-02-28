namespace Beskar.CodeAnalytics.Data.Indexes.Search;

public sealed class BTreeSearchQuery<TKey>
   where TKey : unmanaged, IComparable<TKey>
{
   public required TKey[] Keys { get; set; }

   public BTreeSearchQueryType Type { get; set; } = BTreeSearchQueryType.ExactMatch;

   public long Limit { get; set; } = long.MaxValue;
}

public enum BTreeSearchQueryType
{
   /// <summary>
   /// Value is any of the provided keys
   /// </summary>
   ExactMatch = 1,
   /// <summary>
   /// Keys must be length of two, target value must be between the two keys
   /// </summary>
   Between = 2,
   /// <summary>
   /// Keys must be 1, target value is greater than the key
   /// </summary>
   GreaterThan = 3,
   /// <summary>
   /// Keys must be 1, target value is less than the key
   /// </summary>
   LessThan = 4,
}
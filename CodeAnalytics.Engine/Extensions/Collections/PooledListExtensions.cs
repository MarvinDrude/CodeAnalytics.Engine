using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Extensions.Collections;

public static class PooledListExtensions
{
   public static void Resolve(this PooledList<StringId> strings, Dictionary<int, string> result)
   {
      foreach (ref var item in strings)
      {
         item.Resolve(result);
      }
   }
   
   public static void Resolve(this PooledSet<StringId> strings, Dictionary<int, string> result)
   {
      foreach (ref var item in strings)
      {
         item.Resolve(result);
      }
   }
}
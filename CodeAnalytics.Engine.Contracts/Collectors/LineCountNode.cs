using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Collectors;

public sealed class LineCountNode
{
   public required Dictionary<StringId, LineCountStats> StatsPerFile { get; init; }
   
   public void Merge(LineCountNode source)
   {
      foreach (var (key, value) in source.StatsPerFile)
      {
         if (StatsPerFile.TryGetValue(key, out var stats))
         {
            stats.Merge(value);
            continue;
         }

         StatsPerFile[key] = value;
      }
   }
}
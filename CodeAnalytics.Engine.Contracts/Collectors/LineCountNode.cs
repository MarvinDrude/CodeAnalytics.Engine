using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Collectors;

public sealed class LineCountNode
{
   public required Dictionary<StringId, LineCountStats> StatsPerFile { get; init; }
   
   public required Dictionary<StringId, LineCountStats> StatsPerProject { get; init; }

   public LineCountStats GetTotal()
   {
      var total = new LineCountStats()
      {
         ProjectId = StringId.Empty,
         CodeCount = 0,
         LineCount = 0
      };

      foreach (var (_, stats) in StatsPerProject)
      {
         total.CodeCount += stats.CodeCount;
         total.LineCount += stats.LineCount;
      }
      
      return total;
   }
   
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
      
      foreach (var (key, value) in source.StatsPerProject)
      {
         if (StatsPerProject.TryGetValue(key, out var stats))
         {
            stats.Merge(value);
            continue;
         }

         StatsPerProject[key] = value;
      }
   }
}
namespace CodeAnalytics.Engine.Contracts.Collectors;

public sealed class LineCountStats
{
   public int LineCount { get; set; }
   public int CodeCount { get; set; }
   
   public void Merge(LineCountStats source)
   {
      LineCount += source.LineCount;
      CodeCount += source.CodeCount;
   }
}
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Occurrences;

public sealed class ProjectOccurrence 
   : IEquatable<ProjectOccurrence>
{
   public StringId PathId { get; init; } = StringId.Empty;
   
   public Dictionary<StringId, FileOccurrence> FileOccurrences { get; init; } = [];

   public FileOccurrence GetOrCreate(StringId pathId)
   {
      if (!FileOccurrences.TryGetValue(pathId, out var fileOccurrence))
      {
         fileOccurrence = FileOccurrences[pathId] = new FileOccurrence();
      }

      return fileOccurrence;
   }
   
   public bool Equals(ProjectOccurrence? other)
   {
      return other is not null && PathId.Equals(other.PathId);
   }

   public override bool Equals(object? obj)
   {
      return ReferenceEquals(this, obj) || obj is ProjectOccurrence other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(PathId);
   }
   
   public static bool operator ==(ProjectOccurrence left, ProjectOccurrence right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(ProjectOccurrence left, ProjectOccurrence right)
   {
      return !(left == right);
   }
}
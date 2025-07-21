using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Contracts.Occurrences;

public sealed class FileOccurrence 
   : IEquatable<FileOccurrence>
{
   public StringId PathId { get; init; } = StringId.Empty;
   public List<NodeOccurrence> LineOccurrences { get; init; } = [];

   public bool Equals(FileOccurrence? other)
   {
      return other is not null && PathId.Equals(other.PathId);
   }

   public override bool Equals(object? obj)
   {
      return obj is FileOccurrence other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(PathId);
   }

   public static bool operator ==(FileOccurrence left, FileOccurrence right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(FileOccurrence left, FileOccurrence right)
   {
      return !(left == right);
   }
}
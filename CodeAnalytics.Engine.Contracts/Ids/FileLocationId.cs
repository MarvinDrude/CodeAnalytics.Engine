using System.Runtime.InteropServices;

namespace CodeAnalytics.Engine.Contracts.Ids;

[StructLayout(LayoutKind.Auto)]
public readonly struct FileLocationId
   : IEquatable<FileLocationId>
{
   public readonly StringId FileId;
   public readonly int SpanIndex;

   public FileLocationId(StringId fileId, int spanIndex)
   {
      FileId = fileId;
      SpanIndex = spanIndex;
   }

   public bool Equals(FileLocationId other)
   {
      return FileId.Equals(other.FileId) && SpanIndex == other.SpanIndex;
   }

   public override bool Equals(object? obj)
   {
      return obj is FileLocationId other && Equals(other);
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(FileId, SpanIndex);
   }

   public static bool operator ==(FileLocationId left, FileLocationId right)
   {
      return left.Equals(right);
   }

   public static bool operator !=(FileLocationId left, FileLocationId right)
   {
      return !(left == right);
   }
}
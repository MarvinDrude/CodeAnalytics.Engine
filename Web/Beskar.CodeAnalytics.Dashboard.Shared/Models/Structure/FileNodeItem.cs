using Beskar.CodeAnalytics.Data.Enums.Structure;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Models.Structure;

public sealed class FileNodeItem : FileSystemItem
{
   public required FileKind Kind { get; set; }
}
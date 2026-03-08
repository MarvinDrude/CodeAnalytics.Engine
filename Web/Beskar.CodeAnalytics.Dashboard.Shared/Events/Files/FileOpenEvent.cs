namespace Beskar.CodeAnalytics.Dashboard.Shared.Events.Files;

public sealed class FileOpenEvent
{
   public required uint FileId { get; init; }
}
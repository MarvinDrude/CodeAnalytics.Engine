using Beskar.CodeAnalytics.Dashboard.Shared.Models.Syntax;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Events.Files;

public sealed class LocationNavigateEvent
{
   public required TokenLocationModel Location { get; set; }
}
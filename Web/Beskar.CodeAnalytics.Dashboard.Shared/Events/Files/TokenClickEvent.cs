using Beskar.CodeAnalytics.Data.Entities.Structure;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Events.Files;

public sealed class TokenClickEvent
{
   public SyntaxTokenSpec Token { get; set; }
   
   public int LineNumber { get; set; }
}
using Beskar.CodeAnalytics.Data.Entities.Structure;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Models.Syntax;

public sealed class SourceExplorerTokenInfo : SourceExplorerTokenInfoBase
{
   public required SyntaxTokenSpec Token { get; set; }
}

public sealed class SourceExplorerTokenEmptyInfo 
   : SourceExplorerTokenInfoBase;

public abstract class SourceExplorerTokenInfoBase;
using Beskar.CodeAnalytics.Data.Entities.Structure;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Models.Syntax;

public sealed class SourceExplorerTokenInfo : ISourceExplorerTokenInfoBase
{
   public bool IsEmpty => false;
   
   public required SyntaxTokenSpec Token { get; set; }
   
   public required TokenLocationModel[] Locations { get; set; }
}

public sealed class SourceExplorerTokenEmptyInfo
   : ISourceExplorerTokenInfoBase
{
   public bool IsEmpty => true;
   
   public SyntaxTokenSpec Token { get; } = new();
   
   public TokenLocationModel[] Locations { get; } = [];
}

public interface ISourceExplorerTokenInfoBase
{
   public bool IsEmpty { get; }
   
   public SyntaxTokenSpec Token { get; }
   
   public TokenLocationModel[] Locations { get; }
}
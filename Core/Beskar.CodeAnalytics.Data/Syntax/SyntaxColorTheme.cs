using Beskar.CodeAnalytics.Data.Entities.Structure;

namespace Beskar.CodeAnalytics.Data.Syntax;

public abstract class SyntaxColorTheme
{
   public abstract string GetTokenColor(SyntaxTokenSpec token);
}
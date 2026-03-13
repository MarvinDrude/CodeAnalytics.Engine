using Beskar.CodeAnalytics.Data.Entities.Symbols;

namespace Beskar.CodeAnalytics.Dashboard.Shared.Models.Syntax;

public sealed class TokenLocationModel
{
   public required SymbolLocationSpec Location { get; set; }
   
   public required string PreviewLine { get; set; }
}
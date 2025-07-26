using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Constants;

public static class SymbolDisplayFormats
{
   public static SymbolDisplayFormat NameWithGenerics { get; } =
      new(
         globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
         typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly,
         genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
         miscellaneousOptions:
         SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers |
         SymbolDisplayMiscellaneousOptions.UseSpecialTypes);
}
using Beskar.CodeAnalytics.Data.Enums.Syntax;
using Microsoft.CodeAnalysis.Classification;

namespace Beskar.CodeAnalytics.Collector.Source;

public static class TokenColorizer
{
   public static SyntaxColor Determine(ClassifiedSpan span)
   {
      return span.ClassificationType switch
      {
         ClassificationTypeNames.NameSpa
      };
   }
}
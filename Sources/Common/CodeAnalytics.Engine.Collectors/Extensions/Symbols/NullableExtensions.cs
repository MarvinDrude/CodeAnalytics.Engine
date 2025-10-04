using CodeAnalytics.Engine.Storage.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collectors.Extensions.Symbols;

public static class NullableExtensions
{
   extension(NullableAnnotation annotation)
   {
      public NullAnnotation ToType()
      {
         return annotation switch
         {
            NullableAnnotation.Annotated => NullAnnotation.Annotated,
            NullableAnnotation.NotAnnotated => NullAnnotation.NotAnnotated,
            NullableAnnotation.None => NullAnnotation.None,
            _ => throw new ArgumentOutOfRangeException(nameof(annotation), annotation, null)
         };
      }
   }
}
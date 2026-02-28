using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Extensions;

public static class TypeKindExtensions
{
   extension(TypeKind kind)
   {
      public TypeStorageKind ToStorage()
      {
         return kind switch
         {
            TypeKind.Unknown => TypeStorageKind.Unknown,
            TypeKind.Array => TypeStorageKind.Array,
            TypeKind.Class => TypeStorageKind.Class,
            TypeKind.Delegate => TypeStorageKind.Delegate,
            TypeKind.Dynamic => TypeStorageKind.Dynamic,
            TypeKind.Enum => TypeStorageKind.Enum,
            TypeKind.Error => TypeStorageKind.Error,
            TypeKind.Interface => TypeStorageKind.Interface,
            TypeKind.Module => TypeStorageKind.Module,
            TypeKind.Pointer => TypeStorageKind.Pointer,
            TypeKind.Struct => TypeStorageKind.Struct,
            TypeKind.TypeParameter => TypeStorageKind.TypeParameter,
            TypeKind.Submission => TypeStorageKind.Submission,
            TypeKind.FunctionPointer => TypeStorageKind.FunctionPointer,
            _ => TypeStorageKind.Unknown
         };
      }
   }
}
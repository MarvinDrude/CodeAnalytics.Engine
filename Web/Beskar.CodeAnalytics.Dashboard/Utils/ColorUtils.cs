using Beskar.CodeAnalytics.Data.Enums.Symbols;

namespace Beskar.CodeAnalytics.Dashboard.Utils;

public static class ColorUtils
{
   public static (string SymbolColor, string FontColor) GetSymbolColor(SymbolType type, TypeStorageKind? kind = null)
   {
      return type switch
      {
         SymbolType.Field => ("--kind-field", "--kind-field-text"),
         SymbolType.Method => ("--kind-method", "--kind-method-text"),
         SymbolType.Parameter => ("--kind-parameter", "--kind-parameter-text"),
         SymbolType.Property => ("--kind-property", "--kind-property-text"),
         SymbolType.TypeParameter => ("--kind-type-parameter", "--kind-type-parameter-text"),
         SymbolType.NamedType => GetNamedTypeColor(kind ?? TypeStorageKind.Class),
         _ when type.IsType => GetTypeColor(kind ?? TypeStorageKind.Class),
         _ => ("--kind-unknown", "--kind-unknown-text")
      };
   }

   private static (string SymbolColor, string FontColor) GetNamedTypeColor(TypeStorageKind kind)
   {
      return kind switch
      {
         TypeStorageKind.Class => ("--kind-class", "--kind-class-text"),
         TypeStorageKind.Enum => ("--kind-enum", "--kind-enum-text"),
         TypeStorageKind.Interface => ("--kind-interface", "--kind-interface-text"),
         TypeStorageKind.Struct => ("--kind-struct", "--kind-struct-text"),
         TypeStorageKind.Delegate => ("--kind-delegate", "--kind-delegate-text"),
         TypeStorageKind.Module => ("--kind-module", "--kind-module-text"),
         _ => ("--kind-unknown", "--kind-unknown-text")
      };
   }

   private static (string SymbolColor, string FontColor) GetTypeColor(TypeStorageKind kind)
   {
      return kind switch
      {
         TypeStorageKind.Array => ("--kind-array", "--kind-array-text"),
         TypeStorageKind.Pointer => ("--kind-pointer", "--kind-pointer-text"),
         TypeStorageKind.Dynamic => ("--kind-dynamic", "--kind-dynamic-text"),
         TypeStorageKind.FunctionPointer => ("--kind-function-pointer", "--kind-function-pointer-text"),
         _ => ("--kind-unknown", "--kind-unknown-text")
      };
   }
}
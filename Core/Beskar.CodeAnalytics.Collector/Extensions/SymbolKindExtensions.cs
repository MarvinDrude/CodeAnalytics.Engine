using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Extensions;

public static class SymbolKindExtensions
{
   extension(SymbolKind kind)
   {
      public SymbolType ToStorage()
      {
         return kind switch
         {
            SymbolKind.Alias => SymbolType.Alias,
            SymbolKind.ArrayType => SymbolType.ArrayType,
            SymbolKind.Assembly => SymbolType.Assembly,
            SymbolKind.DynamicType => SymbolType.DynamicType,
            SymbolKind.ErrorType => SymbolType.ErrorType,
            SymbolKind.Event => SymbolType.Event,
            SymbolKind.Field => SymbolType.Field,
            SymbolKind.Label => SymbolType.Label,
            SymbolKind.Local => SymbolType.Local,
            SymbolKind.Method => SymbolType.Method,
            SymbolKind.NetModule => SymbolType.NetModule,
            SymbolKind.NamedType => SymbolType.NamedType,
            SymbolKind.Namespace => SymbolType.Namespace,
            SymbolKind.Parameter => SymbolType.Parameter,
            SymbolKind.PointerType => SymbolType.PointerType,
            SymbolKind.Property => SymbolType.Property,
            SymbolKind.RangeVariable => SymbolType.RangeVariable,
            SymbolKind.TypeParameter => SymbolType.TypeParameter,
            SymbolKind.Preprocessing => SymbolType.Preprocessing,
            SymbolKind.Discard => SymbolType.Discard,
            SymbolKind.FunctionPointerType => SymbolType.FunctionPointerType,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
         };
      }
   }
}
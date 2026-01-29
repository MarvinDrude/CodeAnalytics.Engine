namespace Beskar.CodeAnalytics.Storage.Enums.Symbols;

public enum SymbolKind : byte
{
   Alias = 0,
   ArrayType = 1,
   Assembly = 2,
   DynamicType = 3,
   ErrorType = 4,
   Event = 5,
   Field = 6,
   Label = 7,
   Local = 8,
   Method = 9,
   NetModule = 10,
   NamedType = 11,
   Namespace = 12,
   Parameter = 13,
   PointerType = 14,
   Property = 15,
   RangeVariable = 16,
   TypeParameter = 17,
   Preprocessing = 18,
   Discard = 19,
   FunctionPointerType = 20
}
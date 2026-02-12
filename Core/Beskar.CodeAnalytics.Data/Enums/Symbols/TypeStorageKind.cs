namespace Beskar.CodeAnalytics.Data.Enums.Symbols;

public enum TypeStorageKind : byte
{
   Unknown = 0,
   Array = 1,
   Class = 2,
   Delegate = 3,
   Dynamic = 4,
   Enum = 5,
   Error = 6,
   Interface = 7,
   Module = 8,
   Pointer = 9,
   Struct = 10,
   Structure = Struct,
   TypeParameter = 11,
   Submission = 12,
   FunctionPointer = 13,
   Extension = 14
}
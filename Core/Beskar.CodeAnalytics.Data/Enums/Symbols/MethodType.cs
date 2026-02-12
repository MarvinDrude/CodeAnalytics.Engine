namespace Beskar.CodeAnalytics.Data.Enums.Symbols;

public enum MethodType : byte
{
   AnonymousFunction = 0,
   LambdaMethod = AnonymousFunction,
   Constructor = 1,
   Conversion = 2,
   DelegateInvoke = 3,
   Destructor = 4,
   EventAdd = 5,
   EventRaise = 6,
   EventRemove = 7,
   ExplicitInterfaceImplementation = 8,
   UserDefinedOperator = 9,
   Ordinary = 10,
   PropertyGet = 11,
   PropertySet = 12,
   ReducedExtension = 13,
   SharedConstructor = 14,
   StaticConstructor = SharedConstructor,
   BuiltinOperator = 15,
   DeclareMethod = 16,
   LocalFunction = 17,
   FunctionPointerSignature = 18
}
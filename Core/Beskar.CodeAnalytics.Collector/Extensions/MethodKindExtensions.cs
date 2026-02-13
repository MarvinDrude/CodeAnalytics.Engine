using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Extensions;

public static class MethodKindExtensions
{
   extension(MethodKind kind)
   {
      public MethodType ToStorage()
      {
         return kind switch
         {
            MethodKind.AnonymousFunction => MethodType.AnonymousFunction,
            MethodKind.Constructor => MethodType.Constructor,
            MethodKind.Conversion => MethodType.Conversion,
            MethodKind.DelegateInvoke => MethodType.DelegateInvoke,
            MethodKind.Destructor => MethodType.Destructor,
            MethodKind.EventAdd => MethodType.EventAdd,
            MethodKind.EventRaise => MethodType.EventRaise,
            MethodKind.EventRemove => MethodType.EventRemove,
            MethodKind.ExplicitInterfaceImplementation => MethodType.ExplicitInterfaceImplementation,
            MethodKind.UserDefinedOperator => MethodType.UserDefinedOperator,
            MethodKind.Ordinary => MethodType.Ordinary,
            MethodKind.PropertyGet => MethodType.PropertyGet,
            MethodKind.PropertySet => MethodType.PropertySet,
            MethodKind.ReducedExtension => MethodType.ReducedExtension,
            MethodKind.StaticConstructor => MethodType.StaticConstructor,
            MethodKind.BuiltinOperator => MethodType.BuiltinOperator,
            MethodKind.DeclareMethod => MethodType.DeclareMethod,
            MethodKind.LocalFunction => MethodType.LocalFunction,
            MethodKind.FunctionPointerSignature => MethodType.FunctionPointerSignature,
            _ => MethodType.Ordinary
         };
      }
   }
}
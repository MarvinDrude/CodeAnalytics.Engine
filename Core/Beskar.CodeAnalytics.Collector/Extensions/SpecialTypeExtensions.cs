using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Extensions;

public static class SpecialTypeExtensions
{
   extension(SpecialType type)
   {
      public SpecialTypeKind ToStorage()
      {
         return type switch
         {
            SpecialType.None => SpecialTypeKind.None,
            SpecialType.System_Object => SpecialTypeKind.SystemObject,
            SpecialType.System_Enum => SpecialTypeKind.SystemEnum,
            SpecialType.System_MulticastDelegate => SpecialTypeKind.SystemMulticastDelegate,
            SpecialType.System_Delegate => SpecialTypeKind.SystemDelegate,
            SpecialType.System_ValueType => SpecialTypeKind.SystemValueType,
            SpecialType.System_Void => SpecialTypeKind.SystemVoid,
            SpecialType.System_Boolean => SpecialTypeKind.SystemBoolean,
            SpecialType.System_Char => SpecialTypeKind.SystemChar,
            SpecialType.System_SByte => SpecialTypeKind.SystemSByte,
            SpecialType.System_Byte => SpecialTypeKind.SystemByte,
            SpecialType.System_Int16 => SpecialTypeKind.SystemInt16,
            SpecialType.System_UInt16 => SpecialTypeKind.SystemUInt16,
            SpecialType.System_Int32 => SpecialTypeKind.SystemInt32,
            SpecialType.System_UInt32 => SpecialTypeKind.SystemUInt32,
            SpecialType.System_Int64 => SpecialTypeKind.SystemInt64,
            SpecialType.System_UInt64 => SpecialTypeKind.SystemUInt64,
            SpecialType.System_Decimal => SpecialTypeKind.SystemDecimal,
            SpecialType.System_Single => SpecialTypeKind.SystemSingle,
            SpecialType.System_Double => SpecialTypeKind.SystemDouble,
            SpecialType.System_String => SpecialTypeKind.SystemString,
            SpecialType.System_IntPtr => SpecialTypeKind.SystemIntPtr,
            SpecialType.System_UIntPtr => SpecialTypeKind.SystemUIntPtr,
            SpecialType.System_Array => SpecialTypeKind.SystemArray,
            SpecialType.System_Collections_IEnumerable => SpecialTypeKind.SystemCollectionsIEnumerable,
            SpecialType.System_Collections_Generic_IEnumerable_T => SpecialTypeKind.SystemCollectionsGenericIEnumerableT,
            SpecialType.System_Collections_Generic_IList_T => SpecialTypeKind.SystemCollectionsGenericIListT,
            SpecialType.System_Collections_Generic_ICollection_T => SpecialTypeKind.SystemCollectionsGenericICollectionT,
            SpecialType.System_Collections_IEnumerator => SpecialTypeKind.SystemCollectionsIEnumerator,
            SpecialType.System_Collections_Generic_IEnumerator_T => SpecialTypeKind.SystemCollectionsGenericIEnumeratorT,
            SpecialType.System_Collections_Generic_IReadOnlyList_T => SpecialTypeKind.SystemCollectionsGenericIReadOnlyListT,
            SpecialType.System_Collections_Generic_IReadOnlyCollection_T => SpecialTypeKind.SystemCollectionsGenericIReadOnlyCollectionT,
            SpecialType.System_Nullable_T => SpecialTypeKind.SystemNullableT,
            SpecialType.System_DateTime => SpecialTypeKind.SystemDateTime,
            SpecialType.System_Runtime_CompilerServices_IsVolatile => SpecialTypeKind.SystemRuntimeCompilerServicesIsVolatile,
            SpecialType.System_IDisposable => SpecialTypeKind.SystemIDisposable,
            SpecialType.System_TypedReference => SpecialTypeKind.SystemTypedReference,
            SpecialType.System_ArgIterator => SpecialTypeKind.SystemArgIterator,
            SpecialType.System_RuntimeArgumentHandle => SpecialTypeKind.SystemRuntimeArgumentHandle,
            SpecialType.System_RuntimeFieldHandle => SpecialTypeKind.SystemRuntimeFieldHandle,
            SpecialType.System_RuntimeMethodHandle => SpecialTypeKind.SystemRuntimeMethodHandle,
            SpecialType.System_RuntimeTypeHandle => SpecialTypeKind.SystemRuntimeTypeHandle,
            SpecialType.System_IAsyncResult => SpecialTypeKind.SystemIAsyncResult,
            SpecialType.System_AsyncCallback => SpecialTypeKind.SystemAsyncCallback,
            SpecialType.System_Runtime_CompilerServices_RuntimeFeature => SpecialTypeKind.SystemRuntimeCompilerServicesRuntimeFeature,
            SpecialType.System_Runtime_CompilerServices_PreserveBaseOverridesAttribute => SpecialTypeKind.SystemRuntimeCompilerServicesPreserveBaseOverridesAttribute,
            SpecialType.System_Runtime_CompilerServices_InlineArrayAttribute => SpecialTypeKind.SystemRuntimeCompilerServicesInlineArrayAttribute,
            _ => SpecialTypeKind.None
         };
      }
   }
}
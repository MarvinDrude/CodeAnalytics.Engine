using System.Collections.Immutable;
using Beskar.CodeAnalytics.Collector.Extensions;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Storage.Entities.Edges;
using Beskar.CodeAnalytics.Storage.Entities.Misc;
using Beskar.CodeAnalytics.Storage.Entities.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Symbols.Discovery;

public static class TypeDiscovery
{
   public static async Task<bool> Discover(DiscoverContext context, ulong id)
   {
      if (context.Symbol is not ITypeSymbol typeSymbol)
      {
         return false;
      }

      var batch = context.DiscoveryBatch;

      ulong baseTypeId = 0;
      var hasBaseType = false;
      if (typeSymbol.BaseType is not null
          && UniqueIdentifier.Create(typeSymbol.BaseType) is { } basePath)
      {
         var stringDefinition = batch.StringDefinitions.GetStringDefinition(basePath);
         baseTypeId = batch.Identifiers.GetDeterministicId(basePath, stringDefinition);
         
         hasBaseType = true;
      }
      
      batch.WriteDiscoveryEdges(id, typeSymbol.Interfaces, EdgeType.DirectInterface);
      batch.WriteDiscoveryEdges(id, typeSymbol.AllInterfaces, EdgeType.AllInterface);
      
      var typeDefinition = new TypeSymbolDefinition()
      {
         SymbolId = id,
         BaseTypeId = baseTypeId,
         
         Kind = typeSymbol.TypeKind.ToStorage(),
         SpecialType = typeSymbol.SpecialType.ToStorage(),
         
         AllInterfaces = new StorageSlice<TypeSymbolDefinition>(-1, -1),
         DirectInterfaces = new StorageSlice<TypeSymbolDefinition>(-1, -1),
         
         HasBaseType = hasBaseType,
         IsReadOnly = typeSymbol.IsReadOnly,
         IsRecord = typeSymbol.IsRecord,
         IsReferenceType = typeSymbol.IsReferenceType,
         IsRefLikeType = typeSymbol.IsRefLikeType,
         IsTupleType = typeSymbol.IsTupleType,
         IsUnmanagedType = typeSymbol.IsUnmanagedType,
         IsValueType = typeSymbol.IsValueType,
      };
      await batch.TypeSymbolWriter.Write(id, typeDefinition);
      
      return typeSymbol switch
      {
         INamedTypeSymbol => await DiscoverNamedType(context, id),
         ITypeParameterSymbol => await DiscoverTypeParameter(context, id),
         _ => true,
      };
   }

   public static async Task<bool> DiscoverNamedType(DiscoverContext context, ulong id)
   {
      if (context.Symbol is not INamedTypeSymbol typeSymbol)
      {
         return false;
      }

      var batch = context.DiscoveryBatch;

      ulong enumUnderlyingTypeId = 0;
      var isEnum = false;
      
      if (typeSymbol.EnumUnderlyingType is not null
          && UniqueIdentifier.Create(typeSymbol.EnumUnderlyingType) is { } underlyingPath)
      {
         var stringDefinition = batch.StringDefinitions.GetStringDefinition(underlyingPath);
         enumUnderlyingTypeId = batch.Identifiers.GetDeterministicId(underlyingPath, stringDefinition);
         
         isEnum = true;
      }
      
      batch.WriteDiscoveryEdges(id, typeSymbol.TypeParameters, EdgeType.TypeParameter);
      batch.WriteDiscoveryEdges(id, typeSymbol.InstanceConstructors, EdgeType.InstanceConstructor);
      batch.WriteDiscoveryEdges(id, typeSymbol.StaticConstructors, EdgeType.StaticConstructor);

      var methods = typeSymbol.GetMembers()
         .OfType<IMethodSymbol>()
         .Where(m => m.MethodKind is not (MethodKind.Constructor or MethodKind.StaticConstructor))
         .ToImmutableArray();

      batch.WriteDiscoveryEdges(id, methods, EdgeType.Method);
      
      var definition = new NamedTypeSymbolDefinition()
      {
         SymbolId = id,
         IsFileLocal =  typeSymbol.IsFileLocal,
         
         IsEnum = isEnum,
         EnumUnderlyingTypeId = enumUnderlyingTypeId,
         
         TypeParameters = new StorageSlice<TypeParameterSymbolDefinition>(-1, -1),
         InstanceConstructors = new StorageSlice<MethodSymbolDefinition>(-1, -1),
         StaticConstructors = new StorageSlice<MethodSymbolDefinition>(-1, -1),
         Methods = new StorageSlice<MethodSymbolDefinition>(-1, -1),
      };
      
      await batch.NamedTypeSymbolWriter.Write(id, definition);
      return true;
   }

   public static async Task<bool> DiscoverTypeParameter(DiscoverContext context, ulong id)
   {
      if (context.Symbol is not ITypeParameterSymbol typeSymbol)
      {
         return false;
      }

      var batch = context.DiscoveryBatch;
      
      batch.WriteDiscoveryEdges(id, typeSymbol.ConstraintTypes, EdgeType.ConstraintType);

      var definition = new TypeParameterSymbolDefinition()
      {
         SymbolId = id,
         Ordinal = typeSymbol.Ordinal,
         
         ConstraintTypes = new StorageSlice<TypeSymbolDefinition>(-1, -1),
         AllowsRefLikeType = typeSymbol.AllowsRefLikeType,
         HasConstructorConstraint = typeSymbol.HasConstructorConstraint,
         HasNotNullConstraint = typeSymbol.HasNotNullConstraint,
         HasReferenceTypeConstraint = typeSymbol.HasReferenceTypeConstraint,
         HasValueTypeConstraint = typeSymbol.HasValueTypeConstraint,
         HasUnmanagedTypeConstraint = typeSymbol.HasUnmanagedTypeConstraint,
      };
      await batch.TypeParameterSymbolWriter.Write(id, definition);
      
      return true;
   }
}
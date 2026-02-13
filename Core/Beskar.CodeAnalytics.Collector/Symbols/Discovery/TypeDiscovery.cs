using System.Collections.Immutable;
using Beskar.CodeAnalytics.Collector.Extensions;
using Beskar.CodeAnalytics.Collector.Identifiers;
using Beskar.CodeAnalytics.Collector.Projects.Models;
using Beskar.CodeAnalytics.Data.Entities.Misc;
using Beskar.CodeAnalytics.Data.Entities.Symbols;
using Beskar.CodeAnalytics.Data.Enums.Symbols;
using Microsoft.CodeAnalysis;

namespace Beskar.CodeAnalytics.Collector.Symbols.Discovery;

public static class TypeDiscovery
{
   public static async Task<bool> Discover(DiscoverContext context, uint id)
   {
      if (context.Symbol is not ITypeSymbol typeSymbol)
      {
         return false;
      }

      var batch = context.DiscoveryBatch;

      uint baseTypeId = 0;
      var hasBaseType = false;
      if (typeSymbol.BaseType is not null
          && UniqueIdentifier.Create(typeSymbol.BaseType) is { } basePath)
      {
         var stringDefinition = batch.StringDefinitions.GetStringFileView(basePath);
         baseTypeId = batch.Identifiers.GenerateIdentifier(basePath, stringDefinition);
         
         hasBaseType = true;
      }
      
      batch.WriteDiscoveryEdges(id, typeSymbol.Interfaces, SymbolEdgeType.DirectInterface);
      batch.WriteDiscoveryEdges(id, typeSymbol.AllInterfaces, SymbolEdgeType.AllInterface);
      
      var typeDefinition = new TypeSymbolSpec()
      {
         SymbolId = id,
         BaseTypeId = baseTypeId,
         
         Kind = typeSymbol.TypeKind.ToStorage(),
         SpecialType = typeSymbol.SpecialType.ToStorage(),
         
         AllInterfaces = new StorageView<TypeSymbolSpec>(-1, -1),
         DirectInterfaces = new StorageView<TypeSymbolSpec>(-1, -1),
         
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

   public static async Task<bool> DiscoverNamedType(DiscoverContext context, uint id)
   {
      if (context.Symbol is not INamedTypeSymbol typeSymbol)
      {
         return false;
      }

      var batch = context.DiscoveryBatch;

      uint enumUnderlyingTypeId = 0;
      var isEnum = false;
      
      if (typeSymbol.EnumUnderlyingType is not null
          && UniqueIdentifier.Create(typeSymbol.EnumUnderlyingType) is { } underlyingPath)
      {
         var stringDefinition = batch.StringDefinitions.GetStringFileView(underlyingPath);
         enumUnderlyingTypeId = batch.Identifiers.GenerateIdentifier(underlyingPath, stringDefinition);
         
         isEnum = true;
      }
      
      batch.WriteDiscoveryEdges(id, typeSymbol.TypeParameters, SymbolEdgeType.TypeParameter);
      batch.WriteDiscoveryEdges(id, typeSymbol.InstanceConstructors, SymbolEdgeType.InstanceConstructor);
      batch.WriteDiscoveryEdges(id, typeSymbol.StaticConstructors, SymbolEdgeType.StaticConstructor);

      var methods = typeSymbol.GetMembers()
         .OfType<IMethodSymbol>()
         .Where(m => m.MethodKind is not (MethodKind.Constructor or MethodKind.StaticConstructor))
         .ToImmutableArray();

      batch.WriteDiscoveryEdges(id, methods, SymbolEdgeType.Method);
      
      var definition = new NamedTypeSymbolSpec()
      {
         SymbolId = id,
         IsFileLocal =  typeSymbol.IsFileLocal,
         
         IsEnum = isEnum,
         EnumUnderlyingTypeId = enumUnderlyingTypeId,
         
         TypeParameters = new StorageView<TypeParameterSymbolSpec>(-1, -1),
         InstanceConstructors = new StorageView<MethodSymbolSpec>(-1, -1),
         StaticConstructors = new StorageView<MethodSymbolSpec>(-1, -1),
         Methods = new StorageView<MethodSymbolSpec>(-1, -1),
      };
      
      await batch.NamedTypeSymbolWriter.Write(id, definition);
      return true;
   }

   public static async Task<bool> DiscoverTypeParameter(DiscoverContext context, uint id)
   {
      if (context.Symbol is not ITypeParameterSymbol typeSymbol)
      {
         return false;
      }

      var batch = context.DiscoveryBatch;
      
      batch.WriteDiscoveryEdges(id, typeSymbol.ConstraintTypes, SymbolEdgeType.ConstraintType);

      var definition = new TypeParameterSymbolSpec()
      {
         SymbolId = id,
         Ordinal = typeSymbol.Ordinal,
         
         ConstraintTypes = new StorageView<TypeSymbolSpec>(-1, -1),
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
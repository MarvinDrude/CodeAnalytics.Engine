using System.Collections.Immutable;
using CodeAnalytics.Engine.Collector.Archetypes.Interfaces;
using CodeAnalytics.Engine.Collector.Archetypes.Members;
using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Common;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Ids;
using CodeAnalytics.Engine.Extensions.Symbols;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Types;

public sealed class TypeCollector 
   : IComponentCollector<TypeComponent, INamedTypeSymbol>
{
   public static bool TryParse(
      INamedTypeSymbol symbol, CollectContext context, out TypeComponent component)
   {
      var store = context.Store;
      component = new TypeComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol)
      };

      ParseInterfaces(symbol, context, ref component);
      
      var constructors = symbol.GetMethods(x => x.MethodKind is MethodKind.Constructor or MethodKind.StaticConstructor);
      var methods = symbol.GetMethods(x => x.MethodKind is MethodKind.Ordinary or MethodKind.ExplicitInterfaceImplementation);
      var properties = symbol.GetProperties(_ => true);
      var fields = symbol.GetFields(_ => true);
      
      using var constructorIds = ParseMembers<IMethodSymbol, ConstructorArchetype, ConstructorArchetypeCollector>
         (symbol, constructors, context, ref component);
      using var methodIds = ParseMembers<IMethodSymbol, MethodArchetype, MethodArchetypeCollector>
         (symbol, methods, context, ref component);
      using var propertyIds = ParseMembers<IPropertySymbol, PropertyArchetype, PropertyArchetypeCollector>
         (symbol, properties, context, ref component);
      using var fieldIds = ParseMembers<IFieldSymbol, FieldArchetype, FieldArchetypeCollector>
         (symbol, fields, context, ref component);
      
      component.ConstructorIds.AddRange(constructorIds);
      component.MethodIds.AddRange(methodIds);
      component.PropertyIds.AddRange(propertyIds);
      component.FieldIds.AddRange(fieldIds);
      
      ref var attributeSet = ref component.AttributeIds;
      AttributeCollector.Apply(ref attributeSet, symbol.GetAttributes(), context);
      
      return true;
   }

   private static PooledSet<NodeId> ParseMembers<TSymbol, TArchetype, TArchetypeParser>(
      INamedTypeSymbol symbol,
      ImmutableArray<TSymbol> members,
      CollectContext context,
      ref TypeComponent component)
      where TSymbol : ISymbol
      where TArchetype : IArchetype, IEquatable<TArchetype>
      where TArchetypeParser : IArchetypeCollector<TArchetype, TSymbol>
   {
      var store = context.Store;
      var result = new PooledSet<NodeId>(members.Length);
      
      foreach (var member in members)
      {
         var nodeId = NodeId.Empty;

         if (context.AddSubComponentsImmediately
             && TArchetypeParser.TryParse(member, context, out var archetype))
         {
            nodeId = archetype.NodeId;
            TArchetypeParser.AddArchetype(store, ref archetype);
         }

         if (nodeId.IsEmpty && !context.AddSubComponentsImmediately)
         {
            nodeId = store.NodeIdStore.GetOrAdd(member);
         }

         if (!nodeId.IsEmpty)
         {
            result.Add(nodeId);
         }
      }

      return result;
   }
   
   private static void ParseInterfaces(
      INamedTypeSymbol symbol, CollectContext context, ref TypeComponent component)
   {
      var store = context.Store;

      foreach (var interFace in symbol.AllInterfaces)
      {
         var nodeId = store.NodeIdStore.GetOrAdd(interFace.OriginalDefinition);
         component.InterfaceIds.Add(nodeId);
      }

      foreach (var interFace in symbol.Interfaces)
      {
         var nodeId = store.NodeIdStore.GetOrAdd(interFace.OriginalDefinition);
         component.DirectInterfaceIds.Add(nodeId);
      }
   }
}
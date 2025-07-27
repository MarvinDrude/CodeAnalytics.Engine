﻿using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Merges.Types;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Types;

public sealed class EnumCollector 
   : IComponentCollector<EnumComponent, INamedTypeSymbol>
{
   public static bool TryParse(
      INamedTypeSymbol symbol, CollectContext context, out EnumComponent component)
   {
      var store = context.Store;
      component = new EnumComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol),
      };

      if (symbol.EnumUnderlyingType is { } underlying)
      {
         component.UnderlyingTypeId = store.NodeIdStore.GetOrAdd(underlying.OriginalDefinition);
      }
      else
      {
         return false;
      }
      
      foreach (var value in symbol.GetMembers())
      {
         var definition = value.OriginalDefinition;
         
         component.ValueIds.Add(store.NodeIdStore.GetOrAdd(definition));

         if (context.AddSubComponentsImmediately 
             && EnumValueCollector.TryParse(definition, context, out var valueComponent))
         {
            var pool = context.Store.ComponentStore.GetOrCreatePool<EnumValueComponent, EnumValueMerger>();
            pool.Add(ref valueComponent);
         }
      }
      
      return true;
   }
}
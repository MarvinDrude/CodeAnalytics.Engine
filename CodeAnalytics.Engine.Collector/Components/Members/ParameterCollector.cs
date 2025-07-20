using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Common;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Enums.Components;
using CodeAnalytics.Engine.Extensions.Symbols;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Members;

public sealed class ParameterCollector
   : IComponentCollector<ParameterComponent, IParameterSymbol>
{
   public static bool TryParse(
      IParameterSymbol symbol, CollectContext context, out ParameterComponent component)
   {
      var store = context.Store;

      if (symbol.ContainingSymbol is not IMethodSymbol method)
      {
         component = default;
         return false;
      }

      var parameterId = symbol.GenerateParameterId(method);
      
      component = new ParameterComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(parameterId),
         Modifiers = GetModifiers(symbol),
         TypeId = store.NodeIdStore.GetOrAdd(symbol.Type.OriginalDefinition),
      };
      
      AttributeCollector.Apply(ref component.AttributeIds, symbol.GetAttributes(), context);

      return true;
   }

   private static ParameterModifier GetModifiers(IParameterSymbol symbol)
   {
      var modifiers = ParameterModifier.None;
      
      if (symbol.RefKind == RefKind.In) modifiers |= ParameterModifier.In;
      if (symbol.RefKind == RefKind.Out) modifiers |= ParameterModifier.Out;
      if (symbol.RefKind == RefKind.Ref) modifiers |= ParameterModifier.Ref;
      
      if (symbol.RefKind is RefKind.RefReadOnly or RefKind.RefReadOnlyParameter)
      {
         modifiers |= ParameterModifier.Ref;
         modifiers |= ParameterModifier.ReadOnly;
      }

      if (symbol.ScopedKind is ScopedKind.ScopedRef or ScopedKind.ScopedValue)
      {
         modifiers |= ParameterModifier.Scoped;
      }

      if (symbol.IsThis)
      {
         modifiers |= ParameterModifier.This;
      }
      
      return modifiers;
   }
}
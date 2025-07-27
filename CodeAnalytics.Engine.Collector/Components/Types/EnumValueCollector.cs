using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Collector.Components.Interfaces;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Components.Types;
using Microsoft.CodeAnalysis;

namespace CodeAnalytics.Engine.Collector.Components.Types;

public sealed class EnumValueCollector 
   : IComponentCollector<EnumValueComponent, ISymbol>
{
   public static bool TryParse(
      ISymbol symbol, CollectContext context, out EnumValueComponent component)
   {
      if (symbol is not IFieldSymbol { HasConstantValue: true } field)
      {
         component = default;
         return false;
      }
      
      var store = context.Store;
      component = new EnumValueComponent()
      {
         Id = store.NodeIdStore.GetOrAdd(symbol),
         Flags = new PackedBools(),
      };
      
      var constant = field.ConstantValue;
      var enumType = field.ContainingType;
      var underlying = enumType.EnumUnderlyingType?.SpecialType;

      if (underlying is SpecialType.System_UInt64)
      {
         component.UValue = Convert.ToUInt64(constant);
         component.IsULong = true;
      }
      else
      {
         component.Value = Convert.ToInt64(constant);
         component.IsULong = false;
      }
      
      component.Name = context.Store.StringIdStore.GetOrAdd(field.Name);
      
      return true;
   }
}
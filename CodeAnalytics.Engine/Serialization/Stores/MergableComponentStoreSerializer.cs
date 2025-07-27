using System.Diagnostics.CodeAnalysis;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Components;
using CodeAnalytics.Engine.Contracts.Components.Common;
using CodeAnalytics.Engine.Contracts.Components.Inerfaces;
using CodeAnalytics.Engine.Contracts.Components.Members;
using CodeAnalytics.Engine.Contracts.Components.Types;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Merges.Common;
using CodeAnalytics.Engine.Merges.Interfaces;
using CodeAnalytics.Engine.Merges.Members;
using CodeAnalytics.Engine.Merges.Types;
using CodeAnalytics.Engine.Serialization.Components.Common;
using CodeAnalytics.Engine.Serialization.Components.Members;
using CodeAnalytics.Engine.Serialization.Components.Types;

namespace CodeAnalytics.Engine.Serialization.Stores;

public sealed class MergableComponentStoreSerializer : ISerializer<MergableComponentStore>
{
   public static void Serialize(ref ByteWriter writer, ref MergableComponentStore ob)
   {
      Serialize<SymbolComponent, SymbolMerger, SymbolSerializer>(ref writer, ob);
      
      Serialize<ClassComponent, ClassMerger, ClassSerializer>(ref writer, ob);
      Serialize<InterfaceComponent, InterfaceMerger, InterfaceSerializer>(ref writer, ob);
      Serialize<StructComponent, StructMerger, StructSerializer>(ref writer, ob);
      Serialize<EnumComponent, EnumMerger, EnumSerializer>(ref writer, ob);
      Serialize<EnumValueComponent, EnumValueMerger, EnumValueSerializer>(ref writer, ob);
      Serialize<TypeComponent, TypeMerger, TypeSerializer>(ref writer, ob);
      
      Serialize<MethodComponent, MethodMerger, MethodSerializer>(ref writer, ob);
      Serialize<PropertyComponent, PropertyMerger, PropertySerializer>(ref writer, ob);
      Serialize<FieldComponent, FieldMerger, FieldSerializer>(ref writer, ob);
      Serialize<ConstructorComponent, ConstructorMerger, ConstructorSerializer>(ref writer, ob);
      Serialize<ParameterComponent, ParameterMerger, ParameterSerializer>(ref writer, ob);
      Serialize<MemberComponent, MemberMerger, MemberSerializer>(ref writer, ob);
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out MergableComponentStore ob)
   {
      ob = new MergableComponentStore(1_000);

      if (!TryDeserialize<SymbolComponent, SymbolMerger, SymbolSerializer>(ref reader, ob)
          // types
          || !TryDeserialize<ClassComponent, ClassMerger, ClassSerializer>(ref reader, ob)
          || !TryDeserialize<InterfaceComponent, InterfaceMerger, InterfaceSerializer>(ref reader, ob)
          || !TryDeserialize<StructComponent, StructMerger, StructSerializer>(ref reader, ob)
          || !TryDeserialize<EnumComponent, EnumMerger, EnumSerializer>(ref reader, ob)
          || !TryDeserialize<EnumValueComponent, EnumValueMerger, EnumValueSerializer>(ref reader, ob)
          || !TryDeserialize<TypeComponent, TypeMerger, TypeSerializer>(ref reader, ob)
          // members
          || !TryDeserialize<MethodComponent, MethodMerger, MethodSerializer>(ref reader, ob)
          || !TryDeserialize<PropertyComponent, PropertyMerger, PropertySerializer>(ref reader, ob)
          || !TryDeserialize<FieldComponent, FieldMerger, FieldSerializer>(ref reader, ob)
          || !TryDeserialize<ConstructorComponent, ConstructorMerger, ConstructorSerializer>(ref reader, ob)
          || !TryDeserialize<ParameterComponent, ParameterMerger, ParameterSerializer>(ref reader, ob)
          || !TryDeserialize<MemberComponent, MemberMerger, MemberSerializer>(ref reader, ob))
      {
         return false;
      }
      
      return true;
   }

   private static void Serialize<TComponent, TMerger, TSerializer>(
      ref ByteWriter writer, MergableComponentStore store)
      where TComponent : IComponent, IEquatable<TComponent>
      where TMerger : IComponentMerger<TComponent>
      where TSerializer : ISerializer<TComponent>
   {
      var pool = store.GetOrCreatePool<TComponent, TMerger>();
      writer.WriteLittleEndian(pool.Count);
      
      foreach (ref var component in pool.Entries)
      {
         TSerializer.Serialize(ref writer, ref component);
      }
   }

   private static bool TryDeserialize<TComponent, TMerger, TSerializer>(
      ref ByteReader reader, MergableComponentStore store)
      where TComponent : IComponent, IEquatable<TComponent>
      where TMerger : IComponentMerger<TComponent>
      where TSerializer : ISerializer<TComponent>
   {
      var pool = store.GetOrCreatePool<TComponent, TMerger>();
      var count = reader.ReadLittleEndian<int>();

      if (count == 0) return true;

      for (var e = 0; e < count; e++)
      {
         if (!TSerializer.TryDeserialize(ref reader, out var component))
         {
            return false;
         }

         pool.Add(ref component);
      }
      
      return true;
   }
}
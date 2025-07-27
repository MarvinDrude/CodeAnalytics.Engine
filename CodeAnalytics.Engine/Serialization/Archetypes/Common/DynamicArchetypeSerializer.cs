using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Archetypes.Enums;
using CodeAnalytics.Engine.Contracts.Archetypes.Interfaces;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Archetypes.Members;
using CodeAnalytics.Engine.Serialization.Archetypes.Types;

namespace CodeAnalytics.Engine.Serialization.Archetypes.Common;

public sealed class DynamicArchetypeSerializer : ISerializer<List<IArchetype>>
{
   public static void Serialize(ref ByteWriter writer, ref List<IArchetype> ob)
   {
      var span = CollectionsMarshal.AsSpan(ob);
      writer.WriteLittleEndian(span.Length);
      
      foreach (ref var archetype in span)
      {
         switch (archetype)
         {
            case ConstructorArchetype constr:
               writer.WriteLittleEndian(ArchetypeKind.Constructor);
               ConstructorArchetypeSerializer.Serialize(ref writer, ref constr);
               break;
            case FieldArchetype field:
               writer.WriteLittleEndian(ArchetypeKind.Field);
               FieldArchetypeSerializer.Serialize(ref writer, ref field);
               break;
            case MethodArchetype method:
               writer.WriteLittleEndian(ArchetypeKind.Method);
               MethodArchetypeSerializer.Serialize(ref writer, ref method);
               break;
            case PropertyArchetype property:
               writer.WriteLittleEndian(ArchetypeKind.Property);
               PropertyArchetypeSerializer.Serialize(ref writer, ref property);
               break;
            
            case ClassArchetype cls:
               writer.WriteLittleEndian(ArchetypeKind.Class);
               ClassArchetypeSerializer.Serialize(ref writer, ref cls);
               break;
            case StructArchetype strc:
               writer.WriteLittleEndian(ArchetypeKind.Struct);
               StructArchetypeSerializer.Serialize(ref writer, ref strc);
               break;
            case InterfaceArchetype interfa:
               writer.WriteLittleEndian(ArchetypeKind.Interface);
               InterfaceArchetypeSerializer.Serialize(ref writer, ref interfa);
               break;
            case EnumArchetype enu:
               writer.WriteLittleEndian(ArchetypeKind.Enum);
               EnumArchetypeSerializer.Serialize(ref writer, ref enu);
               break;
            case EnumValueArchetype enumValue:
               writer.WriteLittleEndian(ArchetypeKind.EnumValue);
               EnumValueArchetypeSerializer.Serialize(ref writer, ref enumValue);
               break;
         }
      }
   }

   public static bool TryDeserialize(ref ByteReader reader, [MaybeNullWhen(false)] out List<IArchetype> ob)
   {
      List<IArchetype> result = [];
      var length = reader.ReadLittleEndian<int>();

      for (var i = 0; i < length; i++)
      {
         var kind = reader.ReadLittleEndian<ArchetypeKind>();
         var isSuccess = kind switch
         {
            ArchetypeKind.Constructor => ConstructorArchetypeSerializer.TryDeserialize(ref reader, out var constr) && AddRet(result, constr),
            ArchetypeKind.Field => FieldArchetypeSerializer.TryDeserialize(ref reader, out var constr) && AddRet(result, constr),
            ArchetypeKind.Property => PropertyArchetypeSerializer.TryDeserialize(ref reader, out var constr) && AddRet(result, constr),
            ArchetypeKind.Method => MethodArchetypeSerializer.TryDeserialize(ref reader, out var constr) && AddRet(result, constr),
            
            ArchetypeKind.Class => ClassArchetypeSerializer.TryDeserialize(ref reader, out var constr) && AddRet(result, constr),
            ArchetypeKind.Struct => StructArchetypeSerializer.TryDeserialize(ref reader, out var constr) && AddRet(result, constr),
            ArchetypeKind.Interface => InterfaceArchetypeSerializer.TryDeserialize(ref reader, out var constr) && AddRet(result, constr),
            ArchetypeKind.Enum => EnumArchetypeSerializer.TryDeserialize(ref reader, out var constr) && AddRet(result, constr),
            ArchetypeKind.EnumValue => EnumValueArchetypeSerializer.TryDeserialize(ref reader, out var constr) && AddRet(result, constr),
            _ => false,
         };

         if (!isSuccess)
         {
            ob = null;
            return false;
         }
      }
      
      ob = result;
      return true;
   }

   private static bool AddRet<T>(List<T> list, T value)
   {
      list.Add(value);
      return true;
   }
}
using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Components.Common;
using CodeAnalytics.Engine.Serialization.Components.Members;

namespace CodeAnalytics.Engine.Serialization.Archetypes.Members;

public sealed class FieldArchetypeSerializer : ISerializer<FieldArchetype>
{
   public static void Serialize(ref ByteWriter writer, ref FieldArchetype ob)
   {
      SymbolSerializer.Serialize(ref writer, ref ob.Symbol);
      MemberSerializer.Serialize(ref writer, ref ob.Member);
      FieldSerializer.Serialize(ref writer, ref ob.Field);
   }

   public static bool TryDeserialize(ref ByteReader reader, out FieldArchetype ob)
   {
      if (!SymbolSerializer.TryDeserialize(ref reader, out var symbol)
          || !MemberSerializer.TryDeserialize(ref reader, out var member)
          || !FieldSerializer.TryDeserialize(ref reader, out var specific))
      {
         ob = default;
         return false;
      }

      ob = new FieldArchetype()
      {
         Symbol = symbol,
         Member = member,
         Field = specific
      };
      return true;
   }
}
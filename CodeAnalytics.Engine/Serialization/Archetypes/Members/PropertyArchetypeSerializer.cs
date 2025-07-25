using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Components.Common;
using CodeAnalytics.Engine.Serialization.Components.Members;
using CodeAnalytics.Engine.Serialization.Components.Types;

namespace CodeAnalytics.Engine.Serialization.Archetypes.Members;

public sealed class PropertyArchetypeSerializer : ISerializer<PropertyArchetype>
{
   public static void Serialize(ref ByteWriter writer, ref PropertyArchetype ob)
   {
      SymbolSerializer.Serialize(ref writer, ref ob.Symbol);
      MemberSerializer.Serialize(ref writer, ref ob.Member);
      PropertySerializer.Serialize(ref writer, ref ob.Property);
   }

   public static bool TryDeserialize(ref ByteReader reader, out PropertyArchetype ob)
   {
      if (!SymbolSerializer.TryDeserialize(ref reader, out var symbol)
          || !MemberSerializer.TryDeserialize(ref reader, out var member)
          || !PropertySerializer.TryDeserialize(ref reader, out var specific))
      {
         ob = default;
         return false;
      }

      ob = new PropertyArchetype()
      {
         Symbol = symbol,
         Member = member,
         Property = specific
      };
      return true;
   }
}
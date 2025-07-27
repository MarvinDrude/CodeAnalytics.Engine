using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Components.Common;
using CodeAnalytics.Engine.Serialization.Components.Types;

namespace CodeAnalytics.Engine.Serialization.Archetypes.Types;

public sealed class EnumValueArchetypeSerializer : ISerializer<EnumValueArchetype>
{
   public static void Serialize(ref ByteWriter writer, ref EnumValueArchetype ob)
   {
      SymbolSerializer.Serialize(ref writer, ref ob.Symbol);
      EnumValueSerializer.Serialize(ref writer, ref ob.EnumValue);
   }

   public static bool TryDeserialize(ref ByteReader reader, out EnumValueArchetype ob)
   {
      if (!SymbolSerializer.TryDeserialize(ref reader, out var symbol)
          || !EnumValueSerializer.TryDeserialize(ref reader, out var type))
      {
         ob = default;
         return false;
      }

      ob = new EnumValueArchetype()
      {
         Symbol = symbol,
         EnumValue = type,
      };
      return true;
   }
}
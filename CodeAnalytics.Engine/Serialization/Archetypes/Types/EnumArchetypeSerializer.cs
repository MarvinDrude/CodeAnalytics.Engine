using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Components.Common;
using CodeAnalytics.Engine.Serialization.Components.Types;

namespace CodeAnalytics.Engine.Serialization.Archetypes.Types;

public sealed class EnumArchetypeSerializer : ISerializer<EnumArchetype>
{
   public static void Serialize(ref ByteWriter writer, ref EnumArchetype ob)
   {
      SymbolSerializer.Serialize(ref writer, ref ob.Symbol);
      EnumSerializer.Serialize(ref writer, ref ob.Enum);
   }

   public static bool TryDeserialize(ref ByteReader reader, out EnumArchetype ob)
   {
      if (!SymbolSerializer.TryDeserialize(ref reader, out var symbol)
          || !EnumSerializer.TryDeserialize(ref reader, out var specific))
      {
         ob = default;
         return false;
      }

      ob = new EnumArchetype()
      {
         Symbol = symbol,
         Enum = specific
      };
      return true;
   }
}
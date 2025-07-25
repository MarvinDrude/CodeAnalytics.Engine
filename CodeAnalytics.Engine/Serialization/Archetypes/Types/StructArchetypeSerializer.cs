using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Components.Common;
using CodeAnalytics.Engine.Serialization.Components.Types;

namespace CodeAnalytics.Engine.Serialization.Archetypes.Types;

public sealed class StructArchetypeSerializer : ISerializer<StructArchetype>
{
   public static void Serialize(ref ByteWriter writer, ref StructArchetype ob)
   {
      SymbolSerializer.Serialize(ref writer, ref ob.Symbol);
      TypeSerializer.Serialize(ref writer, ref ob.Type);
      StructSerializer.Serialize(ref writer, ref ob.Struct);
   }

   public static bool TryDeserialize(ref ByteReader reader, out StructArchetype ob)
   {
      if (!SymbolSerializer.TryDeserialize(ref reader, out var symbol)
          || !TypeSerializer.TryDeserialize(ref reader, out var type)
          || !StructSerializer.TryDeserialize(ref reader, out var specific))
      {
         ob = default;
         return false;
      }

      ob = new StructArchetype()
      {
         Symbol = symbol,
         Type = type,
         Struct = specific
      };
      return true;
   }
}
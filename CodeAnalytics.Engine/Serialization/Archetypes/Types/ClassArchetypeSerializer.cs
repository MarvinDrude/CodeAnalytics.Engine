using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Components.Common;
using CodeAnalytics.Engine.Serialization.Components.Types;

namespace CodeAnalytics.Engine.Serialization.Archetypes.Types;

public sealed class ClassArchetypeSerializer : ISerializer<ClassArchetype>
{
   public static void Serialize(ref ByteWriter writer, ref ClassArchetype ob)
   {
      SymbolSerializer.Serialize(ref writer, ref ob.Symbol);
      TypeSerializer.Serialize(ref writer, ref ob.Type);
      ClassSerializer.Serialize(ref writer, ref ob.Class);
   }

   public static bool TryDeserialize(ref ByteReader reader, out ClassArchetype ob)
   {
      if (!SymbolSerializer.TryDeserialize(ref reader, out var symbol)
          || !TypeSerializer.TryDeserialize(ref reader, out var type)
          || !ClassSerializer.TryDeserialize(ref reader, out var specific))
      {
         ob = default;
         return false;
      }

      ob = new ClassArchetype()
      {
         Symbol = symbol,
         Type = type,
         Class = specific
      };
      return true;
   }
}
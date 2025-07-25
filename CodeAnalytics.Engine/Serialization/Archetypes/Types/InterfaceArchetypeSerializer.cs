using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Archetypes.Types;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Components.Common;
using CodeAnalytics.Engine.Serialization.Components.Types;

namespace CodeAnalytics.Engine.Serialization.Archetypes.Types;

public sealed class InterfaceArchetypeSerializer : ISerializer<InterfaceArchetype>
{
   public static void Serialize(ref ByteWriter writer, ref InterfaceArchetype ob)
   {
      SymbolSerializer.Serialize(ref writer, ref ob.Symbol);
      TypeSerializer.Serialize(ref writer, ref ob.Type);
      InterfaceSerializer.Serialize(ref writer, ref ob.Interface);
   }

   public static bool TryDeserialize(ref ByteReader reader, out InterfaceArchetype ob)
   {
      if (!SymbolSerializer.TryDeserialize(ref reader, out var symbol)
          || !TypeSerializer.TryDeserialize(ref reader, out var type)
          || !InterfaceSerializer.TryDeserialize(ref reader, out var specific))
      {
         ob = default;
         return false;
      }

      ob = new InterfaceArchetype()
      {
         Symbol = symbol,
         Type = type,
         Interface = specific
      };
      return true;
   }
}
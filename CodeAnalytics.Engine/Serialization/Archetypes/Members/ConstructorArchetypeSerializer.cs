using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Components.Common;
using CodeAnalytics.Engine.Serialization.Components.Members;

namespace CodeAnalytics.Engine.Serialization.Archetypes.Members;

public sealed class ConstructorArchetypeSerializer : ISerializer<ConstructorArchetype>
{
   public static void Serialize(ref ByteWriter writer, ref ConstructorArchetype ob)
   {
      SymbolSerializer.Serialize(ref writer, ref ob.Symbol);
      MemberSerializer.Serialize(ref writer, ref ob.Member);
      ConstructorSerializer.Serialize(ref writer, ref ob.Constructor);
   }

   public static bool TryDeserialize(ref ByteReader reader, out ConstructorArchetype ob)
   {
      if (!SymbolSerializer.TryDeserialize(ref reader, out var symbol)
          || !MemberSerializer.TryDeserialize(ref reader, out var member)
          || !ConstructorSerializer.TryDeserialize(ref reader, out var specific))
      {
         ob = default;
         return false;
      }

      ob = new ConstructorArchetype()
      {
         Symbol = symbol,
         Member = member,
         Constructor = specific
      };
      return true;
   }
}
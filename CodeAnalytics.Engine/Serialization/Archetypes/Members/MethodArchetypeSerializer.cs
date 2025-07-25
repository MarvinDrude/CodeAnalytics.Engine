using CodeAnalytics.Engine.Common.Buffers;
using CodeAnalytics.Engine.Contracts.Archetypes.Members;
using CodeAnalytics.Engine.Contracts.Serialization;
using CodeAnalytics.Engine.Serialization.Components.Common;
using CodeAnalytics.Engine.Serialization.Components.Members;

namespace CodeAnalytics.Engine.Serialization.Archetypes.Members;

public sealed class MethodArchetypeSerializer : ISerializer<MethodArchetype>
{
   public static void Serialize(ref ByteWriter writer, ref MethodArchetype ob)
   {
      SymbolSerializer.Serialize(ref writer, ref ob.Symbol);
      MemberSerializer.Serialize(ref writer, ref ob.Member);
      MethodSerializer.Serialize(ref writer, ref ob.Method);
   }

   public static bool TryDeserialize(ref ByteReader reader, out MethodArchetype ob)
   {
      if (!SymbolSerializer.TryDeserialize(ref reader, out var symbol)
          || !MemberSerializer.TryDeserialize(ref reader, out var member)
          || !MethodSerializer.TryDeserialize(ref reader, out var specific))
      {
         ob = default;
         return false;
      }

      ob = new MethodArchetype()
      {
         Symbol = symbol,
         Member = member,
         Method = specific
      };
      return true;
   }
}